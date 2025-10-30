/*
 * Pattern: Facade
 * Obiettivi didattici:
 *   - Fornire un'interfaccia semplificata per un insieme di sottosistemi complessi.
 *   - Centralizzare logiche comuni evitando di esporre dettagli implementativi.
 *   - Favorire l'utilizzo del codice client grazie ad un'API chiara.
 * Istruzioni:
 *   - Completa i TODO per arricchire la facciata con nuove funzionalità.
 */

namespace DesignPatternsTodo2.Patterns;

public static class FacadePattern
{
    public static void Run()
    {
        var travelFacade = new TravelFacade(new FlightService(), new HotelService(), new PaymentService());
        travelFacade.BookTrip("Roma", new DateOnly(2024, 6, 1), 5);

        // TODO: gestisci scenari di errore (es. pagamento rifiutato) direttamente nella facciata.
    }
}

public sealed class TravelFacade
{
    private readonly FlightService _flightService;
    private readonly HotelService _hotelService;
    private readonly PaymentService _paymentService;

    public TravelFacade(FlightService flightService, HotelService hotelService, PaymentService paymentService)
    {
        _flightService = flightService;
        _hotelService = hotelService;
        _paymentService = paymentService;
    }

    public void BookTrip(string destination, DateOnly departureDate, int nights)
    {
        Console.WriteLine("== Prenotazione viaggio ==");
        var flight = _flightService.BookFlight(destination, departureDate);
        var hotel = _hotelService.ReserveRoom(destination, nights);
        var total = flight.Price + hotel.TotalPrice;

        Console.WriteLine($"Totale stimato: {total:C}");
        var paymentResult = _paymentService.Pay(total);

        if (!paymentResult)
        {
            Console.WriteLine("Pagamento non riuscito. Annullamento prenotazione in corso...");
            // TODO: annulla le prenotazioni parziali e notifica l'utente.
            return;
        }

        Console.WriteLine("Viaggio confermato! Buona permanenza.");
    }

    // TODO: aggiungi metodi per servizi extra (es. noleggio auto, assicurazione viaggio).
}

public sealed class FlightService
{
    public FlightBooking BookFlight(string destination, DateOnly departureDate)
    {
        Console.WriteLine($"Prenoto volo per {destination} il {departureDate}");
        return new FlightBooking(destination, departureDate, 120m);
    }
}

public sealed class HotelService
{
    public HotelReservation ReserveRoom(string destination, int nights)
    {
        Console.WriteLine($"Prenoto hotel a {destination} per {nights} notti");
        return new HotelReservation(destination, nights, pricePerNight: 80m);
    }
}

public sealed class PaymentService
{
    public bool Pay(decimal amount)
    {
        Console.WriteLine($"Eseguo pagamento di {amount:C}");
        // TODO: integra logiche reali di pagamento o simulazioni (es. probabilità di fallimento).
        return true;
    }
}

public sealed record FlightBooking(string Destination, DateOnly Date, decimal Price);

public sealed record HotelReservation(string Destination, int Nights, decimal PricePerNight)
{
    public decimal TotalPrice => Nights * PricePerNight;
}
