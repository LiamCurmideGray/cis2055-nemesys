﻿using cis2055_NemesysProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cis2055_NemesysProject.Data.Interfaces
{
    public interface INemesysRepository
    {
        IEnumerable<Report> GetAllReports();
        Report GetReportById(int id);
    }
}