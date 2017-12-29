namespace TrainTrain.Test.Acceptance
{
    public class TrainTopologyGenerator
    {
        public static string With_10_available_seats()
        {
            return "{\"seats\": {" +
                   "\"1A\": {\"booking_reference\": \"\", \"seat_number\": \"1\", \"coach\": \"A\"}, " +
                   "\"2A\": {\"booking_reference\": \"\", \"seat_number\": \"2\", \"coach\": \"A\"}, " +
                   "\"3A\": {\"booking_reference\": \"\", \"seat_number\": \"3\", \"coach\": \"A\"}, " +
                   "\"4A\": {\"booking_reference\": \"\", \"seat_number\": \"4\", \"coach\": \"A\"}, " +
                   "\"5A\": {\"booking_reference\": \"\", \"seat_number\": \"5\", \"coach\": \"A\"}, " +
                   "\"6A\": {\"booking_reference\": \"\", \"seat_number\": \"6\", \"coach\": \"A\"}, " +
                   "\"7A\": {\"booking_reference\": \"\", \"seat_number\": \"7\", \"coach\": \"A\"}, " +
                   "\"8A\": {\"booking_reference\": \"\", \"seat_number\": \"8\", \"coach\": \"A\"}, " +
                   "\"9A\": {\"booking_reference\": \"\", \"seat_number\": \"9\", \"coach\": \"A\"}, " +
                   "\"10A\": {\"booking_reference\": \"\", \"seat_number\": \"10\", \"coach\": \"A\"}}}";
        }

        public static string With_10_seats_and_6_already_reserved()
        {
            return "{\"seats\": {" +
                   "\"1A\": {\"booking_reference\": \"75bcd16\", \"seat_number\": \"1\", \"coach\": \"A\"}, " +
                   "\"2A\": {\"booking_reference\": \"75bcd16\", \"seat_number\": \"2\", \"coach\": \"A\"}, " +
                   "\"3A\": {\"booking_reference\": \"75bcd16\", \"seat_number\": \"3\", \"coach\": \"A\"}, " +
                   "\"4A\": {\"booking_reference\": \"75bcd16\", \"seat_number\": \"4\", \"coach\": \"A\"}, " +
                   "\"5A\": {\"booking_reference\": \"75bcd16\", \"seat_number\": \"5\", \"coach\": \"A\"}, " +
                   "\"6A\": {\"booking_reference\": \"75bcd16\", \"seat_number\": \"6\", \"coach\": \"A\"}, " +
                   "\"7A\": {\"booking_reference\": \"\", \"seat_number\": \"7\", \"coach\": \"A\"}, " +
                   "\"8A\": {\"booking_reference\": \"\", \"seat_number\": \"8\", \"coach\": \"A\"}, " +
                   "\"9A\": {\"booking_reference\": \"\", \"seat_number\": \"9\", \"coach\": \"A\"}, " +
                   "\"10A\": {\"booking_reference\": \"\", \"seat_number\": \"10\", \"coach\": \"A\"}" +
                   "}}";
        }

        public static string With_2_coaches_and_9_seats_already_reserved_in_the_first_coach()
        {
            return "{\"seats\": {" +

                   "\"1A\": {\"booking_reference\": \"75bcd16\", \"seat_number\": \"1\", \"coach\": \"A\"}, " +
                   "\"2A\": {\"booking_reference\": \"75bcd16\", \"seat_number\": \"2\", \"coach\": \"A\"}, " +
                   "\"3A\": {\"booking_reference\": \"75bcd15\", \"seat_number\": \"3\", \"coach\": \"A\"}, " +
                   "\"4A\": {\"booking_reference\": \"75bcd15\", \"seat_number\": \"4\", \"coach\": \"A\"}, " +
                   "\"5A\": {\"booking_reference\": \"75bcd19\", \"seat_number\": \"5\", \"coach\": \"A\"}, " +
                   "\"6A\": {\"booking_reference\": \"75bcd19\", \"seat_number\": \"6\", \"coach\": \"A\"}, " +
                   "\"7A\": {\"booking_reference\": \"a5bcd16\", \"seat_number\": \"7\", \"coach\": \"A\"}, " +
                   "\"8A\": {\"booking_reference\": \"a5bcd16\", \"seat_number\": \"8\", \"coach\": \"A\"}, " +
                   "\"9A\": {\"booking_reference\": \"c5bce14\", \"seat_number\": \"9\", \"coach\": \"A\"}, " +
                   "\"10A\": {\"booking_reference\": \"\", \"seat_number\": \"10\", \"coach\": \"A\"}," +

                   "\"1B\": {\"booking_reference\": \"\", \"seat_number\": \"1\", \"coach\": \"B\"}, " +
                   "\"2B\": {\"booking_reference\": \"\", \"seat_number\": \"2\", \"coach\": \"B\"}, " +
                   "\"3B\": {\"booking_reference\": \"\", \"seat_number\": \"3\", \"coach\": \"B\"}, " +
                   "\"4B\": {\"booking_reference\": \"\", \"seat_number\": \"4\", \"coach\": \"B\"}, " +
                   "\"5B\": {\"booking_reference\": \"\", \"seat_number\": \"5\", \"coach\": \"B\"}, " +
                   "\"6B\": {\"booking_reference\": \"\", \"seat_number\": \"6\", \"coach\": \"B\"}, " +
                   "\"7B\": {\"booking_reference\": \"\", \"seat_number\": \"7\", \"coach\": \"B\"}, " +
                   "\"8B\": {\"booking_reference\": \"\", \"seat_number\": \"8\", \"coach\": \"B\"}, " +
                   "\"9B\": {\"booking_reference\": \"\", \"seat_number\": \"9\", \"coach\": \"B\"}, " +
                   "\"10B\": {\"booking_reference\": \"\", \"seat_number\": \"10\", \"coach\": \"B\"}" +
                   "}}";
        }
    }
}