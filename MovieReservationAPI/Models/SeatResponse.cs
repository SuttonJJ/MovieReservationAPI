namespace MovieReservationAPI.Models;

public class SeatResponse
{
    public int Id { get; set; }
    public string Row { get; set; }
    public int Number { get; set; }
    public string SeatLabel => $"{Row}{Number}";
}