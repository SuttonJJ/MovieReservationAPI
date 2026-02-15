namespace MovieReservationAPI.Models.Entities;

public class Seat
{
    public int Id { get; set; }
    public string Row { get; set; }
    public int Number { get; set; }

    public List<Reservation> Reservations { get; set; }
}