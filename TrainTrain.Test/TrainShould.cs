using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFluent;
using NUnit.Framework;
using TrainTrain.Domain;
using TrainTrain.Infra;
using TrainTrain.Test.Acceptance;

namespace TrainTrain.Test
{
    class TrainShould
    {
        [Test]
        public void Expose_Coaches()
        {
            var train = new Train(TrainDataService.AdaptTrainTopology(TrainTopologyGenerator.With_2_coaches_and_9_seats_already_reserved_in_the_first_coach()));

            Check.That(train.Coaches).HasSize(2);
            Check.That(train.Coaches["A"].Seats).HasSize(10);
            Check.That(train.Coaches["B"].Seats).HasSize(10);
        }
    }
}
