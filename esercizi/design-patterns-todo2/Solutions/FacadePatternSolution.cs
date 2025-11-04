namespace DesignPatternsTodo2.Solutions;

/// <summary>
/// Soluzione del pattern Facade con gestione errori e servizi extra.
/// </summary>
public static class FacadePatternSolution
{
    public static void Run()
    {
        var travelFacade = new TravelFacade(new FlightService(), new HotelService(), new PaymentService(), new CarRentalService(), new InsuranceService());
        travelFacade.BookTrip("Roma", new DateOnly(2024, 6, 1), 5, includeCarRental: true, includeInsurance: true);

        Console.WriteLine();
        travelFacade.BookTrip("Tokyo", new DateOnly(2024, 7, 10), 10, includeCarRental: false, includeInsurance: true);
    }
}

public sealed class TravelFacade
{
    private readonly FlightService _flightService;
    private readonly HotelService _hotelService;
    private readonly PaymentService _paymentService;
    private readonly CarRentalService _carRentalService;
    private readonly InsuranceService _insuranceService;

    public TravelFacade(FlightService flightService, HotelService hotelService, PaymentService paymentService, CarRentalService carRentalService, InsuranceService insuranceService)
    {
        _flightService = flightService;
        _hotelService = hotelService;
        _paymentService = paymentService;
        _carRentalService = carRentalService;
        _insuranceService = insuranceService;
    }

    public void BookTrip(string destination, DateOnly departureDate, int nights, bool includeCarRental, bool includeInsurance)
    {
        Console.WriteLine("== Prenotazione viaggio ==");
        var flight = _flightService.BookFlight(destination, departureDate);
        var hotel = _hotelService.ReserveRoom(destination, nights);
        CarRentalBooking? car = null;
        TravelInsurance? insurance = null;

        if (includeCarRental)
        {
            car = _carRentalService.BookCar(destination, nights);
        }

        if (includeInsurance)
        {
            insurance = _insuranceService.PurchasePolicy(destination, nights);
        }

        var total = flight.Price + hotel.TotalPrice + (car?.TotalPrice ?? 0) + (insurance?.Price ?? 0);
        Console.WriteLine($"Totale stimato: {total:C}");
        var paymentResult = _paymentService.Pay(total);

        if (!paymentResult)
        {
            Console.WriteLine("Pagamento non riuscito. Annullamento prenotazione in corso...");
            _flightService.CancelFlight(flight);
            _hotelService.CancelReservation(hotel);
            if (car is not null)
            {
                _carRentalService.CancelBooking(car);
            }

            if (insurance is not null)
            {
                _insuranceService.CancelPolicy(insurance);
            }

            Console.WriteLine("Prenotazione annullata, nessun addebito effettuato.");
            return;
        }

        Console.WriteLine("Viaggio confermato! Buona permanenza.");
    }
}

public sealed class FlightService
{
    public FlightBooking BookFlight(string destination, DateOnly departureDate)
    {
        Console.WriteLine($"Prenoto volo per {destination} il {departureDate}");
        return new FlightBooking(destination, departureDate, 120m);
    }

    public void CancelFlight(FlightBooking booking)
    {
        Console.WriteLine($"Annullamento volo per {booking.Destination} del {booking.Date}.");
    }
}

public sealed class HotelService
{
    public HotelReservation ReserveRoom(string destination, int nights)
    {
        Console.WriteLine($"Prenoto hotel a {destination} per {nights} notti");
        return new HotelReservation(destination, nights, pricePerNight: 80m);
    }

    public void CancelReservation(HotelReservation reservation)
    {
        Console.WriteLine($"Annullamento hotel a {reservation.Destination}.");
    }
}

public sealed class CarRentalService
{
    public CarRentalBooking BookCar(string destination, int days)
    {
        Console.WriteLine($"Prenoto auto a {destination} per {days} giorni");
        return new CarRentalBooking(destination, days, dailyRate: 35m);
    }

    public void CancelBooking(CarRentalBooking booking)
    {
        Console.WriteLine($"Annullamento noleggio auto a {booking.Destination}.");
    }
}

public sealed class InsuranceService
{
    public TravelInsurance PurchasePolicy(string destination, int nights)
    {
        Console.WriteLine($"Acquisto assicurazione viaggio per {destination}.");
        return new TravelInsurance(destination, nights * 3m);
    }

    public void CancelPolicy(TravelInsurance insurance)
    {
        Console.WriteLine($"Annullamento polizza per {insurance.Destination}.");
    }
}

public sealed class PaymentService
{
    public bool Pay(decimal amount)
    {
        Console.WriteLine($"Eseguo pagamento di {amount:C}");
        // Simuliamo un fallimento quando la cifra supera 400€.
        if (amount > 400m)
        {
            Console.WriteLine("Pagamento rifiutato dalla banca.");
            return false;
        }

        Console.WriteLine("Pagamento approvato.");
        return true;
    }
}

public sealed record FlightBooking(string Destination, DateOnly Date, decimal Price);

public sealed record HotelReservation(string Destination, int Nights, decimal PricePerNight)
{
    public decimal TotalPrice => Nights * PricePerNight;
}

public sealed record CarRentalBooking(string Destination, int Days, decimal DailyRate)
{
    public decimal TotalPrice => Days * DailyRate;
}

public sealed record TravelInsurance(string Destination, decimal Price);
