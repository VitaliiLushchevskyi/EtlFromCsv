CREATE DATABASE TransportationsDB;
GO

USE TransportationsDB;
GO

CREATE TABLE Trips (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    tpep_pickup_datetime DATETIME NOT NULL,
    tpep_dropoff_datetime DATETIME NOT NULL,
    passenger_count INT NOT NULL,
    trip_distance FLOAT NOT NULL,
    store_and_fwd_flag NVARCHAR(3) NOT NULL, 
    PULocationID INT NOT NULL,
    DOLocationID INT NOT NULL,
    fare_amount DECIMAL(10, 2) NOT NULL,
    tip_amount DECIMAL(10, 2) NOT NULL
);
GO

CREATE INDEX IX_PULocationID ON Trips(PULocationID);
GO

CREATE INDEX IX_trip_distance ON Trips(trip_distance);
GO

CREATE INDEX IX_trip_duration ON Trips(tpep_pickup_datetime, tpep_dropoff_datetime);
GO