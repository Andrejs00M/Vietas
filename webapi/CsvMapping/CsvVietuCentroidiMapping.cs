using TinyCsvParser.Mapping;
using webapi.Models.CsvModels;

namespace webapi.CsvMapping
{
    public class CsvVietuCentroidiMapping : CsvMapping<VietuCentroidi>
    {
        public CsvVietuCentroidiMapping() : base()
        {
            MapProperty(0, x => x.Kods);
            MapProperty(1, x => x.Tips_Cd);
            MapProperty(2, x => x.Nosaukums);
            MapProperty(3, x => x.Vkur_Cd);
            MapProperty(4, x => x.Vkur_Tips);
            MapProperty(5, x => x.Std);
            MapProperty(6, x => x.Koord_X);
            MapProperty(7, x => x.Koord_Y);
            MapProperty(8, x => x.Dd_N);
            MapProperty(9, x => x.Dd_E);
        }
    }
}
