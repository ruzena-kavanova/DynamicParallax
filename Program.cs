
#region konstanty
//hmotnost Slunce
double mS = 1.99E30; //[kg]
//gravitacni konstanta
double g = 6.67430E-11; //[m3·kg−1·s−2]
//zarivy vykon Slunce
double zarivy_vykon_slunce = 3.827E26; //[W]
//absolutni magnituda Slunce 
double abs_magS = 4.83;
//[pc]
double pc = 3.08567758E16; //[m]
//ly
double ly = 9460730472580800; //[m]

//presnost
double precision = 0.99;
//treti odmocnina jako exponent
double treti_odmocnina = 1.0/3.0;
double triapul_odmocnina = 1.0/3.5;
#endregion


#region zadane promenne
//uhlova velikost velke poloosy 
double uhlova_velikost_a = 1.25E-3; //[°]
//uhlova velikost velke poloosy 
double rad_a = (Math.PI / 180) * uhlova_velikost_a; //[rad]
//uhlova velikost velke poloosy v obloukovych "
double a = 4.5; //["]
//uhlova velikost male poloosy v obloukovych "
double b = 3.4; //["]
//h = excentricita
double h = Math.Pow(Math.Pow(a,2) - Math.Pow(b,2), 1.0/2.0); //["]

//relativni magnituda primarni slozky
double rel_mag1 = 3.9; //todo set as parameter
//relativni magnituda sekundarni slozky
double rel_mag2 = 5.3; //todo set as parameter 
#endregion


//Vypocet obezne doby
double celkova_plocha_elipsy = Math.PI * a * b;
double plocha_elipticke_vysece = a * b *(Math.Acos(h/a) - (h/Math.Pow(a,2) * Math.Pow(Math.Pow(a,2) - Math.Pow(h,2), 1.0/2.0))); //arccos je vypocten v [rad]

//obezna doba
double t = celkova_plocha_elipsy / plocha_elipticke_vysece * 11; //[roky], aplikace II. Keplerova zákona
Console.WriteLine($"Oběžná doba soustavy: {t:0.##}[roky]");
t *= 365 * 24 * 60 * 60; //[s]

//hmotnost primarni slozky
double m1 = mS; //[kg]
//hmotnost sekundarni slozky
double m2 = mS;
bool do_another_cycle = false;
//citac cyklu
int i = 1; 
do { 
    //vypocet odhadu velke poloosy 
    double a_abs = Math.Pow(g * ((m1 + m2) / (4 * Math.Pow(Math.PI, 2))) * Math.Pow(t, 2), treti_odmocnina); //[m]
    //vypocet odhadu vzdalenosti
    double d = a_abs / Math.Tan(rad_a); //[m]
    //prevod na pc
    d /= pc; //[pc]

    //odhad absolutnich magnitud
    double abs_mag1 = rel_mag1 + 5 - 5 * Math.Log10(d);
    double abs_mag2 = rel_mag2 + 5 - 5 * Math.Log10(d); 

    //odhad zariveho vykonu
    double l1 = Math.Pow(10, (abs_mag1 - abs_magS -2.5 * Math.Log10(zarivy_vykon_slunce)) / -2.5);
    double l2 = Math.Pow(10, (abs_mag2 - abs_magS - 2.5 * Math.Log10(zarivy_vykon_slunce)) / -2.5);

    //odhad nove hmotnosti
    double new_m1 = Math.Pow(l1 / zarivy_vykon_slunce * Math.Pow(mS, 3.5), triapul_odmocnina);
    double new_m2 = Math.Pow(l2 / zarivy_vykon_slunce * Math.Pow(mS, 3.5), triapul_odmocnina);

    do_another_cycle =  new_m1 / m1 < precision || new_m2 / m2 < precision;
    
    Console.WriteLine($"Cyklus č.:{i}");
    Console.WriteLine($"Hmotnost primární složky: {new_m1: 0.00E0}[kg], přesnost: {new_m1 / m1 * 100 : 0.00}%");
    Console.WriteLine($"Absolutní magnituda primární složky: {abs_mag1: 0.00}");
    Console.WriteLine($"Hmotnost sekundární složky: {new_m2: 0.00E0}[kg], přesnost: {new_m2 / m2 * 100: 0.00}%");
    Console.WriteLine($"Absolutní magnituda sekundární složky: {abs_mag2: 0.00}");
    Console.WriteLine($"Vzdálenost soustavy: {d*pc: 0.00E0}[m], {d*pc/ly: 0.00E}[ly]");

    m1 = new_m1;
    m2 = new_m2;

    i++;
}
while(do_another_cycle);

Console.ReadLine();
 

