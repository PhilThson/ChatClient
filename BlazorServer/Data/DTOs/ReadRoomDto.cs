namespace BlazorServer.Data.DTOs
{
    public class ReadRoomDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
        public IEnumerable<ReadParticipantDto> Participants { get; set; }
        public IEnumerable<ReadMessageDto> Messages { get; set; }
    }
}

