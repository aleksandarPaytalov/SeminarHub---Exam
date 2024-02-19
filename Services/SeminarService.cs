using Microsoft.EntityFrameworkCore;
using SeminarHub.Contracts;
using SeminarHub.Data;
using SeminarHub.Data.DataConstants;
using SeminarHub.Data.Models;
using SeminarHub.Models;
using System.Globalization;

namespace SeminarHub.Services
{
    public class SeminarService : ISeminarService
    {
        private readonly SeminarHubDbContext _context;

        public SeminarService(SeminarHubDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<SeminarViewModel>> GetAllSeminarsAsync()
        {
            return await _context.Seminars
                .AsNoTracking()
                .Select(s => new SeminarViewModel()
                {
                    Id = s.Id,
                    Category = s.Category.Name,
                    DateAndTime = s.DateAndTime.ToString(ValidationConstants.DateFormat),
                    Lecturer = s.Lecturer,
                    Organizer = s.Organizer.UserName,
                    Topic = s.Topic
                })
                .ToListAsync();
        }

        public async Task<ICollection<CategoryViewModel>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task AddNewSeminarAsync(AddNewSeminarViewModel model, string userId)
        {
            DateTime date;
            var isValid = DateTime.TryParseExact(model.DateAndTime, ValidationConstants.DateFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            if (isValid)
            {
                var seminar = new Seminar()
                {
                    Topic = model.Topic,
                    DateAndTime = date,
                    CategoryId = model.CategoryId,
                    Details = model.Details,
                    Duration = model.Duration,
                    Lecturer = model.Lecturer,
                    OrganizerId = userId
                };

                await _context.Seminars.AddAsync(seminar);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<SeminarViewModel>> GetAllSeminarsFromCollectionAsync(string userId)
        {
            return await _context.SeminarsParticipants
                .Where(sp => sp.ParticipantId == userId)
                .AsNoTracking()
                .Select(ep => new SeminarViewModel()
                {
                    Id = ep.SeminarId,
                    Category = ep.Seminar.Category.Name,
                    DateAndTime = ep.Seminar.DateAndTime.ToString(ValidationConstants.DateFormat),
                    Lecturer = ep.Seminar.Lecturer,
                    Organizer = ep.Seminar.Organizer.UserName,
                    Topic = ep.Seminar.Topic
                })
                .ToListAsync();
        }

        public async Task<bool> AddToCollectionAsync(int id, string userId, bool isAdded)
        {
            var seminar = await _context.Seminars
                .Where(s => s.Id == id)
                .Include(s => s.SeminarsParticipants)
                .FirstOrDefaultAsync();
            if (seminar == null)
            {
                throw new ArgumentException("Invalid ad ID");
            }
            
            var checkIfAlreadyAdded = seminar.SeminarsParticipants
                .Any(sp => sp.ParticipantId == userId);
            if (!checkIfAlreadyAdded)
            {
                seminar.SeminarsParticipants.Add(new SeminarParticipant()
                {
                    SeminarId = id,
                    ParticipantId = userId
                });
            
                await _context.SaveChangesAsync();
                isAdded = true;
            }
            
            else
            {
                isAdded = false;
            }

            return isAdded;
        }

        public async Task RemoveFromCollectionAsync(int id, string userId)
        {
            var seminar = await _context.Seminars
                .Where(s => s.Id == id)
                .Include(s => s.SeminarsParticipants)
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                throw new ArgumentException("Invalid Id");
            }
            
            var seminarParticipant = seminar.SeminarsParticipants
                .FirstOrDefault(sp => sp.ParticipantId == userId);
            if (seminarParticipant == null)
            {
                throw new ArgumentException("Invalid UserId");
            }

            seminar.SeminarsParticipants.Remove(seminarParticipant);
            await _context.SaveChangesAsync();
        }

        public async Task<AddNewSeminarViewModel?> GetSeminarForEditAsync(int id)
        {
            var modelForEditing = await _context.Seminars
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (modelForEditing == null)
            {
                throw new ArgumentException("Invalid Id");
            }
            
            var categories = await GetAllCategoriesAsync();

            return await _context.Seminars
                .Where(s => s.Id == id)
                .Select(s => new AddNewSeminarViewModel()
                {
                    Topic = modelForEditing.Topic,
                    Lecturer = modelForEditing.Lecturer,
                    Details = modelForEditing.Details,
                    DateAndTime = modelForEditing.DateAndTime.ToString(ValidationConstants.DateFormat),
                    Duration = modelForEditing.Duration,
                    CategoryId = modelForEditing.CategoryId,
                    Categories = categories
                })
                .FirstOrDefaultAsync();
        }

        public async Task EditSeminarAsync(AddNewSeminarViewModel model, int id)
        {
            var seminar = await _context.Seminars.FindAsync(id);

            DateTime eventDate;
            bool isDateValid = DateTime.TryParseExact(model.DateAndTime,
                ValidationConstants.DateFormat,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out eventDate);
            if (!isDateValid)
            {
                throw new ArgumentException($"Invalid date! Format must be: {ValidationConstants.DateFormat}.");
            }

            if (seminar != null)
            {
                seminar.Topic = model.Topic;
                seminar.Lecturer = model.Lecturer;
                seminar.Details = model.Details;
                seminar.DateAndTime = eventDate;
                seminar.Duration = model.Duration;
                seminar.CategoryId = model.CategoryId;

                await _context.SaveChangesAsync();
            }
            
        }

        public async Task<SeminarDetailsViewModel?> GetSeminarDetailsAsync(int id)
        {
            return await _context.Seminars
                .AsNoTracking()
                .Where(s => s.Id == id)
                .Select(s => new SeminarDetailsViewModel()
                {
                    Id = s.Id,
                    Category = s.Category.Name,
                    DateAndTime = s.DateAndTime.ToString(ValidationConstants.DateFormat),
                    Details = s.Details,
                    Duration = s.Duration,
                    Lecturer = s.Lecturer,
                    Organizer = s.Organizer.UserName,
                    Topic = s.Topic
                })
                .FirstOrDefaultAsync();
        }

        public async Task<DeleteSeminarViewModel?> GetSeminarForDeletingAsync(int id)
        {
            return await _context.Seminars
                .Where(s => s.Id == id)
                .Select(s => new DeleteSeminarViewModel()
                {
                    Topic = s.Topic,
                    Id = s.Id,
                    DateAndTime = s.DateAndTime
                })
                .FirstOrDefaultAsync();
        }

        public async Task DeleteSeminarByIdAsync(int id, string userId)
        {
            var seminar = await _context.Seminars
                .Where(s => s.Id == id && s.OrganizerId == userId)
                .FirstOrDefaultAsync();
               

            if (seminar != null)
            {
                _context.Seminars.Remove(seminar);
                await _context.SaveChangesAsync();
            }
        }
    }
}
