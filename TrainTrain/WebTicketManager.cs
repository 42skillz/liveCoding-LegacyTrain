﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TrainTrain.Domain;

namespace TrainTrain
{
    public class WebTicketManager : IReserveSeats
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        private const string UriTrainDataService = "http://localhost:50680";
        private readonly ITrainDataService _trainDataService;
        private readonly IBookingReferenceService _bookingReferenceService;

        public WebTicketManager():this(new TrainDataService(UriTrainDataService), new BookingReferenceService(UriBookingReferenceService))
        {

        }

        public WebTicketManager(ITrainDataService trainDataService, IBookingReferenceService bookingReferenceService)
        {

            _trainDataService = trainDataService;
            _bookingReferenceService = bookingReferenceService;
        }

        public async Task<ReservationAttempt> Reserve(string trainId, int seatsRequestedCount)
        {
            // get the train
            var jsonTrain = await _trainDataService.GetTrain(trainId);

            var train = new Train(jsonTrain, trainId);
            if (train.DoesNotExceedOverallTrainCapacityLimit(seatsRequestedCount))
            {
                var reservationAttempt = train.BuildReservationAttempt(seatsRequestedCount);

                if (reservationAttempt.IsFullfiled)
                {
                    var bookingRef = await _bookingReferenceService.GetBookingReference();

                    reservationAttempt.AssignBookingReference(bookingRef);

                    await _trainDataService.ReserveSeats(trainId, bookingRef, reservationAttempt.AvailableSeats);

                    return reservationAttempt;

                }
            }

            return new ReservationAttempt(seatsRequestedCount, trainId);
        }

        public bool IsFullfiled(int seatsRequestedCount, List<Seat> availableSeats)
        {
            return availableSeats.Count == seatsRequestedCount;
        }
    }
}