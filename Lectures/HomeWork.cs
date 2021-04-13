﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lecture
{
    internal class HomeWork
    {
        const decimal discountForInfant = 50;
        const decimal discountForChildren = 25;
        const decimal discountForNearbyStreat = 15;

        private string PrepareForCheckingNearStreat(string address)
        {
            var words = address.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Skip(1).ToArray();
            var streetAddress = string.Empty;

            foreach (var word in words)
                streetAddress += word;

            return streetAddress;
        }
        private decimal CurrencyExchanger(decimal amount, string currenciesFrom)
        {
            var coefficientEUR = 1.19M;

            var newAmount = currenciesFrom switch
            {
                "EUR" => amount * coefficientEUR,
                _ => amount
            };
            return newAmount;
        }

        private decimal GetFullPrice(
                                    IEnumerable<string> destinations,
                                    IEnumerable<string> clients,
                                    IEnumerable<int> infantsIds,
                                    IEnumerable<int> childrenIds,
                                    IEnumerable<decimal> prices,
                                    IEnumerable<string> currencies)
        {
            decimal fullPrice = default;          

            if (destinations.Count() != clients.Count() || destinations.Count() != prices.Count() || destinations.Count() != currencies.Count())
            {
                return fullPrice;
            }

            var discount = new List<decimal>();
            var listPrices = new List<decimal>();
           
            foreach (var price in prices)
            {
                listPrices.Add(price);
                discount.Add(0);
            }

            for (var index = 0; index < currencies.Count(); index++)
            {
                var currency = currencies.ElementAt(index);
                if (currency != "USD")
                    listPrices[index] = CurrencyExchanger(listPrices[index], currency);
            }

            foreach (var infantId in infantsIds)
            {
                if (infantId <= discount.Count())
                    discount[infantId] += discountForInfant;
            }

            foreach (var childrenId in childrenIds)
            {
                if (childrenId <= discount.Count())
                    discount[childrenId] += discountForChildren;
            }

            var pastAddress = string.Empty;
            var pointer = 0;
            foreach (var destination in destinations)
            {
                if (destination.Contains("Wayne Street"))
                    listPrices[pointer] += 10;

                if (destination.Contains("North Heather Street"))
                    listPrices[pointer] -= 5.36M;

                if (PrepareForCheckingNearStreat(destination) == pastAddress)
                    discount[pointer] += discountForNearbyStreat;

                fullPrice += listPrices[pointer] - ((listPrices[pointer] / 100) * discount[pointer]);

                pointer++;
                pastAddress = PrepareForCheckingNearStreat(destination);
            }
                
            return fullPrice;
        }

        public decimal InvokePriceCalculatiion()
        {
            var destinations = new[]
            {
                "949 Fairfield Court, Madison Heights, MI 48071",
                "367 Wayne Street, Hendersonville, NC 28792",
                "910 North Heather Street, Cocoa, FL 32927",
                "911 North Heather Street, Cocoa, FL 32927",
                "706 Tarkiln Hill Ave, Middleburg, FL 32068",
            };

            var clients = new[]
            {
                "Autumn Baldwin",
                "Jorge Hoffman",
                "Amiah Simmons",
                "Sariah Bennett",
                "Xavier Bowers",
            };

            var infantsIds = new[] { 2 };
            var childrenIds = new[] { 3, 4 };

            var prices = new[] { 100, 25.23m, 58, 23.12m, 125 };
            var currencies = new[] { "USD", "USD", "EUR", "USD", "USD" };

            return GetFullPrice(destinations, clients, infantsIds, childrenIds, prices, currencies);
        }
    }
}
