using BusinessLayer;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer
{
    public class SqlServerCompactRepository : IRepository
    {
        public int GetRegistrationFee(int? yearsExperience)
        {
            List<FeeTableDriven> feeTable = new List<FeeTableDriven>();

            feeTable.Add(new FeeTableDriven() { MinYearExperience = 0, MaxYearExperience = 1, Fee = 500, });
            feeTable.Add(new FeeTableDriven() { MinYearExperience = 2, MaxYearExperience = 3, Fee = 250 });
            feeTable.Add(new FeeTableDriven() { MinYearExperience = 4, MaxYearExperience = 5, Fee = 100 });
            feeTable.Add(new FeeTableDriven() { MinYearExperience = 6, MaxYearExperience = 9, Fee = 50 });

            var result = feeTable.FirstOrDefault(x => x.MaxYearExperience <= yearsExperience
                                                                        && x.MinYearExperience >= yearsExperience);

            return result?.Fee??0;

        }

        public int SaveSpeaker(Speaker speaker)
        {
            //TODO: Save speaker to DB for now. For demo, just assume success and return 1.
            return 1;
        }



    }
}
