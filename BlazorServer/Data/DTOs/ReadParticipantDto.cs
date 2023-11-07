namespace BlazorServer.Data.DTOs
{
    public class ReadParticipantDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public bool IsAdmin { get; set; }
    }
}

