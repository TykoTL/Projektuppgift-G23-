using System;
using System.Collections.Generic;	
using System.Linq;
using SeventWebApplication.Models.VehicleNS;
using SeventWebApplication.Models.Addon;

namespace SeventWebApplication.Models.Booking
{
    public class Booking
    {
        
        public string BookingId { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } 
        public Vehicle Vehicle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IAddon Addon { get; set; }
        public string Status { get; set; } = "Pending"; // la till status så jag kudne n testa bokningar i olika status i viewern, pending e default
        //public IPricingStrategy PricingStrategy { get; set; }

        // Notes 
        // ApprovedBy


        // Add booking constructor 
        public Booking(string bookingId, string userId, Vehicle vehicle, DateTime startDate, DateTime endDate, IAddon addon) /* IPricingStrategy pricingStrateg*/
        {
            BookingId = bookingId;
            UserId = userId;
            Vehicle = vehicle;
            StartDate = startDate;
            EndDate = endDate;
            Addon = addon;
            //PricingStrategy = pricingStrategy;
        }


        //public decimal GetTotalPrice()
        //{
        //    return PricingStrategy.CalculatePrice(this);
        //} 
        // utkommenterad då PricingStrategy inte är implementerad än

        // Status of the booking with Clone() method
    }
}