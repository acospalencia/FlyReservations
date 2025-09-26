using FlyReservations.Endpoints;

namespace FlyReservations.EndPoints
{
    public static class Startup
    {

        public static void UsarEdPoints(this WebApplication app)
        {
            AeropuertoEndpoints.Add(app);
            AvionEndpoints.Add(app);
            FlightEndpoints.Add(app);
            ReservationEndPoints.Add(app);
            RolEndpoints.Add(app);
            SeatEndpoints.Add(app);
        }

    }
}
