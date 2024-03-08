using BoerisCreaciones.Core.Models.MateriasPrimas;
using BoerisCreaciones.Repository.Interfaces;
using BoerisCreaciones.Service.Interfaces;

namespace BoerisCreaciones.Service.Services
{
    public class MateriasPrimasService : IMateriasPrimasService
    {
        private readonly IMateriasPrimasRepository _repository;

        public MateriasPrimasService(IMateriasPrimasRepository repository)
        {
            _repository = repository;
        }

        public List<MateriaPrimaDTO> ListRawMaterials()
        {
            List<MateriaPrimaDTO> materiasPrimas = new List<MateriaPrimaDTO>();

            MateriaPrimaDTO mp1 = new MateriaPrimaDTO(
              5478761,
              30,
              "Palillos de madera",
              5120,
              "Un gran proveedor de madera refinada",
              new DateTime(2021, 7, 26),
              12,
              "Lorem ipsum dolor sit amet consectetur adipisicing elit. Magni, reiciendis dolores! Saepe dicta voluptatibus molestiae placeat delectus quaerat similique quas et. Debitis est et, sed dolore magni necessitatibus deserunt corporis?Lorem ipsum dolor sit amet consectetur, adipisicing elit. Quaerat dignissimos beatae aliquid dicta sint mollitia cum tenetur nobis quidem provident odio eaque veritatis ea, inventore porro laudantium, ab nesciunt facilis."
            );

            MateriaPrimaDTO mp2 = new MateriaPrimaDTO(
              5339875,
              30,
              "Palillos de madera",
              5120,
              "Un gran proveedor de madera refinada",
              new DateTime(2021, 07, 26),
              new DateTime(2021, 07, 28),
              12,
              2
            );

            MateriaPrimaDTO mp3 = new MateriaPrimaDTO(
              4868811,
              30,
              "Palillos de madera",
              5120,
              "Un gran proveedor de madera refinada",
              new DateTime(2021, 07, 26),
              new DateTime(2021, 07, 28),
              new DateTime(2021, 08, 21),
              12,
              2
            );

            MateriaPrimaDTO mp4 = new MateriaPrimaDTO(
              1452098,
              87,
              "Tela blanca",
              8009,
              2,
              "m2",
              "El telar",
              new DateTime(2022, 05, 12),
              "Se debe recoger por la tarde"
            );

            MateriaPrimaDTO mp5 = new MateriaPrimaDTO(
              4378829,
              90,
              "Tela blanca",
              8009,
              2,
              "m2",
              "El telar",
              new DateTime(2022, 05, 12),
              new DateTime(2022, 05, 23)
            );

            MateriaPrimaDTO mp6 = new MateriaPrimaDTO(
              7613755,
              100,
              "Tela blanca",
              8009,
              2,
              "m2",
              "El telar",
              new DateTime(2022, 05, 12),
              new DateTime(2022, 05, 23),
              new DateTime(2022, 09, 02)
            );

            MateriaPrimaDTO mp7 = new MateriaPrimaDTO(
              8374812,
              103,
              "Tela azul",
              7000,
              3,
              "m2",
              "El telar",
              new DateTime(2022, 06, 05),
              "Orden de compra pendiente"
            );

            MateriaPrimaDTO mp8 = new MateriaPrimaDTO(
              1867828,
              105,
              "Cuero sintético",
              6500,
              5,
              "m2",
              "Fábrica de textiles",
              new DateTime(2022, 06, 10),
              new DateTime(2022, 06, 15)
            );

            MateriaPrimaDTO mp9 = new MateriaPrimaDTO(
              4891743,
              110,
              "Hilo de algodón",
              300,
              1000,
              "metros",
              "Hilandería S.A.",
              new DateTime(2022, 06, 20),
              new DateTime(2022, 06, 25),
              new DateTime(2022, 08, 15)
            );

            MateriaPrimaDTO mp10 = new MateriaPrimaDTO(
              3938915,
              93,
              "Cajas de cartón",
              2500,
              "Paquete de 50 unidades",
              new DateTime(2022, 06, 20),
              new DateTime(2022, 06, 22),
              new DateTime(2022, 06, 30),
              50,
              12
            );

            MateriaPrimaDTO mp11 = new MateriaPrimaDTO(
              2722017,
              95,
              "Cajas de cartón",
              2500,
              "Paquete de 50 unidades",
              new DateTime(2022, 06, 20),
              new DateTime(2022, 06, 22),
              new DateTime(2022, 06, 30),
              50,
              5
            );

            MateriaPrimaDTO mp12 = new MateriaPrimaDTO(
              8064685,
              105,
              "Papel de embalaje",
              1500,
              "Paquete de 100 hojas",
              new DateTime(2022, 06, 25),
              new DateTime(2022, 06, 27),
              new DateTime(2022, 07, 05),
              100,
              25
            );

            MateriaPrimaDTO mp13 = new MateriaPrimaDTO(
              5016830,
              110,
              "Papel de embalaje",
              1500,
              "Paquete de 100 hojas",
              new DateTime(2022, 06, 25),
              new DateTime(2022, 06, 27),
              new DateTime(2022, 07, 05),
              100,
              15
            );

            MateriaPrimaDTO mp14 = new MateriaPrimaDTO(
              3579885,
              80,
              "Aceite de cocina",
              40,
              5,
              "litros",
              "Distribuidora de alimentos",
              new DateTime(2023, 08, 15),
              "Producto de alta calidad"
            );

            MateriaPrimaDTO mp15 = new MateriaPrimaDTO(
              4239928,
              85,
              "Aceite de cocina",
              40,
              5,
              "litros",
              "Distribuidora de alimentos",
              new DateTime(2023, 08, 15),
              "Producto de alta calidad"
            );

            MateriaPrimaDTO mp16 = new MateriaPrimaDTO(
              2364081,
              95,
              "Harina de trigo",
              30,
              25,
              "kilogramos",
              "Molino San Pablo",
              new DateTime(2023, 08, 20),
              "Materia prima para panadería"
            );

            MateriaPrimaDTO mp17 = new MateriaPrimaDTO(
              4282411,
              100,
              "Harina de trigo",
              30,
              25,
              "kilogramos",
              "Molino San Pablo",
              new DateTime(2023, 08, 20),
              "Materia prima para panadería"
            );

            materiasPrimas.AddRange(new List<MateriaPrimaDTO>
            {
                mp1,
                mp2,
                mp3,
                mp4,
                mp5,
                mp6,
                mp7,
                mp8,
                mp9,
                mp10,
                mp11,
                mp12,
                mp13,
                mp14,
                mp15,
                mp16,
                mp17
            });

            return materiasPrimas;
        }
    }
}
