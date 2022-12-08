using TripPlanner.DatabaseModels.Models.Enums;

namespace TripPlanner.Logic.DtoModels
{
    public class UpdateStatusModel
    {
        public RequestStatus Status { get; set; }
        public Guid Id { get; set; }
    }
}
