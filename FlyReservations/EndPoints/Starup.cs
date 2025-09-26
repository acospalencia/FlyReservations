using FlyReservations.Endpoints;

namespace FlyReservations.EndPoints
{
    public static class Starup
    {

        public static void UsarEdPoints(this WebApplication app)
        {
            AeropuertoEndpoints.Add(app);
            AvionEndpoints.Add(app);
        }

    }
}
