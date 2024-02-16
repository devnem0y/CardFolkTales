using UnityEngine;

/// <summary>
/// Сюда записываем классификацию карт, которая будет парситься в id
/// ОБЯЗАТЕЛЬНО УКАЗЫВАТЬ НОМЕР, СОБСТВЕННО ЭТО И БУДЕТ id
/// </summary>

public enum Identity // Юниты от 1 до 50 (пока, потом если что пересчитать) Бонусы от 50
{
//TODO: ЮНИТЫ
 [Tooltip ("Штурмовики")]
 Stormtroopers = 1,
 [Tooltip ("Спецназ")]
 Special_forces = 2,
 [Tooltip ("Мотострелки")]
 Motor_rifle = 3,
 
 [Tooltip ("БМП")]
 BMP = 4,
 [Tooltip ("Легкий танк")]
 Light_tank = 5,
 [Tooltip ("Тяжелый танк")]
 Heavy_tank = 6,
 
 [Tooltip ("Миномет")]
 Mortar = 7,
 [Tooltip ("Ствольная артиллерия")]
 Barrel_artillery = 8,
 [Tooltip ("РСЗО")]
 MLRS = 9,
 
 //TODO: БОНУСЫ
 [Tooltip ("Малый ящик")]
 Small_box = 51,
 [Tooltip ("Средний ящик")]
 Medium_box = 57,
 [Tooltip ("Большой ящик")]
 Big_box = 58,
 [Tooltip ("Аптечка")]
 First_aid_kit = 52,
 [Tooltip ("Малое укрытие")]
 Small_Shelter = 53,
 [Tooltip ("Среднее укрытие")]
 Medium_Shelter = 59,
 [Tooltip ("Большое укрытие")]
 Big_Shelter = 60,
 [Tooltip ("Тепловизер")]
 Thermal_imager = 54,
 [Tooltip ("Гранатомет")]
 Grenade_launcher = 55,
 [Tooltip ("Ремкомплект")]
 Repair_kit = 56,
}