using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas.Dto
{
    [AutoMap(typeof(Mapa))]
    public class MapaDto : CamposPadraoCRUDDto
    {
        public string Cabec1 { get; set; }
        public string Cabec2 { get; set; }
        public string Cabec3 { get; set; }
        public string Cabec4 { get; set; }
        public string Cabec5 { get; set; }
        public string Cabec6 { get; set; }
        public string Cabec7 { get; set; }
        public string Cabec8 { get; set; }
        public string Cabec9 { get; set; }
        public string Cabec10 { get; set; }
        public string Cabec11 { get; set; }
        public string Cabec12 { get; set; }
        public string Cabec13 { get; set; }
        public string Cabec14 { get; set; }
        public string Cabec15 { get; set; }
        public string Cabec16 { get; set; }
        public string Cabec17 { get; set; }
        public string Cabec18 { get; set; }
        public string Cabec19 { get; set; }
        public string Cabec20 { get; set; }
        public int? Exame1ID { get; set; }
        public int? Exame2ID { get; set; }
        public int? Exame3ID { get; set; }
        public int? Exame4ID { get; set; }
        public int? Exame5ID { get; set; }
        public int? Exame6ID { get; set; }
        public int? Exame7ID { get; set; }
        public int? Exame8ID { get; set; }
        public int? Exame9ID { get; set; }
        public int? Exame10ID { get; set; }
        public int? Exame11ID { get; set; }
        public int? Exame12ID { get; set; }
        public int? Exame13ID { get; set; }
        public int? Exame14ID { get; set; }
        public int? Exame15ID { get; set; }
        public int? Exame16ID { get; set; }
        public int? Exame17ID { get; set; }
        public int? Exame18ID { get; set; }
        public int? Exame19ID { get; set; }
        public int? Exame20ID { get; set; }

        public static Mapa Mapear(MapaDto input)
        {
            var result = MapearBase<Mapa>(input);
            result.Cabec1 = input.Cabec1;
            result.Cabec2 = input.Cabec2;
            result.Cabec3 = input.Cabec3;
            result.Cabec4 = input.Cabec4;
            result.Cabec5 = input.Cabec5;
            result.Cabec6 = input.Cabec6;
            result.Cabec7 = input.Cabec7;
            result.Cabec8 = input.Cabec8;
            result.Cabec9 = input.Cabec9;
            result.Cabec10 = input.Cabec10;
            result.Cabec11 = input.Cabec11;
            result.Cabec12 = input.Cabec12;
            result.Cabec13 = input.Cabec13;
            result.Cabec14 = input.Cabec14;
            result.Cabec15 = input.Cabec15;
            result.Cabec16 = input.Cabec16;
            result.Cabec17 = input.Cabec17;
            result.Cabec18 = input.Cabec18;
            result.Cabec19 = input.Cabec19;
            result.Cabec20 = input.Cabec20;
            result.Exame1ID = input.Exame1ID;
            result.Exame2ID = input.Exame2ID;
            result.Exame3ID = input.Exame3ID;
            result.Exame4ID = input.Exame4ID;
            result.Exame5ID = input.Exame5ID;
            result.Exame6ID = input.Exame6ID;
            result.Exame7ID = input.Exame7ID;
            result.Exame8ID = input.Exame8ID;
            result.Exame9ID = input.Exame9ID;
            result.Exame10ID = input.Exame10ID;
            result.Exame11ID = input.Exame11ID;
            result.Exame12ID = input.Exame12ID;
            result.Exame13ID = input.Exame13ID;
            result.Exame14ID = input.Exame14ID;
            result.Exame15ID = input.Exame15ID;
            result.Exame16ID = input.Exame16ID;
            result.Exame17ID = input.Exame17ID;
            result.Exame18ID = input.Exame18ID;
            result.Exame19ID = input.Exame19ID;
            result.Exame20ID = input.Exame20ID;

            return result;

        }

        public static MapaDto Mapear(Mapa input)
        {
            var result = MapearBase<MapaDto>(input);
            result.Cabec1 = input.Cabec1;
            result.Cabec2 = input.Cabec2;
            result.Cabec3 = input.Cabec3;
            result.Cabec4 = input.Cabec4;
            result.Cabec5 = input.Cabec5;
            result.Cabec6 = input.Cabec6;
            result.Cabec7 = input.Cabec7;
            result.Cabec8 = input.Cabec8;
            result.Cabec9 = input.Cabec9;
            result.Cabec10 = input.Cabec10;
            result.Cabec11 = input.Cabec11;
            result.Cabec12 = input.Cabec12;
            result.Cabec13 = input.Cabec13;
            result.Cabec14 = input.Cabec14;
            result.Cabec15 = input.Cabec15;
            result.Cabec16 = input.Cabec16;
            result.Cabec17 = input.Cabec17;
            result.Cabec18 = input.Cabec18;
            result.Cabec19 = input.Cabec19;
            result.Cabec20 = input.Cabec20;
            result.Exame1ID = input.Exame1ID;
            result.Exame2ID = input.Exame2ID;
            result.Exame3ID = input.Exame3ID;
            result.Exame4ID = input.Exame4ID;
            result.Exame5ID = input.Exame5ID;
            result.Exame6ID = input.Exame6ID;
            result.Exame7ID = input.Exame7ID;
            result.Exame8ID = input.Exame8ID;
            result.Exame9ID = input.Exame9ID;
            result.Exame10ID = input.Exame10ID;
            result.Exame11ID = input.Exame11ID;
            result.Exame12ID = input.Exame12ID;
            result.Exame13ID = input.Exame13ID;
            result.Exame14ID = input.Exame14ID;
            result.Exame15ID = input.Exame15ID;
            result.Exame16ID = input.Exame16ID;
            result.Exame17ID = input.Exame17ID;
            result.Exame18ID = input.Exame18ID;
            result.Exame19ID = input.Exame19ID;
            result.Exame20ID = input.Exame20ID;

            return result;

        }
    }
}
