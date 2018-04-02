using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Linq;
using TrainTrain.Dal.Entities;

namespace TrainTrain.Dal
{
    public class RepositoryEntity : IRepositoryEntity<TrainEntity>
    {
        public TrainEntity Get(string id)
        {
            using (var db = new TrainTrainContext())
            {
                return db.Trains.Include(train => train.Seats).SingleOrDefault(t => t.TrainId == id);
            }
        }

        public List<TrainEntity> GetAll()
        {
            using (var db = new TrainTrainContext())
            {
                return db.Trains.Include(train => train.Seats).ToList();
            }
        }

        public void Save(TrainEntity entity)
        {
            if (entity == null) return;

            using (var db = new TrainTrainContext())
            {
                db.Trains.AddOrUpdate(entity);
                db.SaveChanges();
            }
        }

        public void SaveAll(TrainEntity[] entities)
        {
            if (entities == null) return;

            using (var db = new TrainTrainContext())
            {
                db.Trains.AddOrUpdate(entities);
                db.SaveChanges();
            }
        }

        public void Remove(string trainId)
        {
            using (var db = new TrainTrainContext())
            {
                var train = db.Trains.Include(t => t.Seats).SingleOrDefault(t => t.TrainId == trainId);
                if (train != null)
                {
                    train.Seats.RemoveAll(t => true);
                    RemoveSeats(db);
                    db.Trains.Remove(train);
                }
                db.SaveChanges();
            }
        }

        private static void RemoveSeats(TrainTrainContext db)
        {
            var seats = from s in db.Seats
                select s;
            foreach (var s in seats)
            {
                db.Seats.Remove(s);
            }
        }

        public void RemoveAll()
        {
            using (var db = new TrainTrainContext())
            {
                var trains = db.Trains.Include(t => t.Seats).ToList();
                if (trains.Any())
                {
                    foreach (var trainEntity in trains)
                    {
                        trainEntity.Seats.RemoveAll(t => true);
                        RemoveSeats(db);
                        db.Trains.Remove(trainEntity);
                    }
                }
                db.SaveChanges();
            }
        }
    }
}