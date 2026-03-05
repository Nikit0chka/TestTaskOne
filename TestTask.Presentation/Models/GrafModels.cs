using System;
using System.Collections.Generic;
using System.Text;
using TestTask.Domain.WeightingAggregate;

namespace TestTaskOne.Models
{
    public class GrafModels
    {
        public DateTime Date { get; set; }          // Дата (или период)
        public double TotalWeight { get; set; }      // Суммарный вес за период
        public int Count { get; set; }                // Количество взвешиваний
    }
}
