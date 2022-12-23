# GoAuto

## Sprendžiamo uždavinio aprašymas

### Sistemos paskirtis

Projekto tikslas - suprojektuoti patogią bei intuityvią svetainę, leidžiančią skaityti bei rašyti atsiliepimus apie konkrečius automobilių modelius.

Veikimo principas – pačią kuriamą platformą sudaro dvi dalys: internetinė aplikacija, kuria naudosis registruoti vartotojai, administratorius, bei aplikacijų programavimo sąsaja (angl. trump. API).

Vartotojas, norėdamas naudotis čia platformą, prisiregistruos prie internetinės aplikacijos ir turės galimybę rašyti automobilių atsiliepimus užpildydamas atsiliepimo formą, kurią sudaro tam tikri automobilio parametrai kaip gamybos metai ar variklio tūris, taip pat matyti visus savo atsiliepimus su kitų svetainės registruotų vartotojų įvertinimais, pats turės galimybę vertinti kitus atsiliepimus. Be to, aplikacija suteiks galimybę naudotis atsiliepimų paieška. Administratorius galės kuruoti vartotojus, t.y. matyti jų sąrašą, konkrečius vartotojus šalinti arba suteikti jiems administratoriaus teises, taip pat peržiūrėti visus arba tam tikro vartotojo atsiliepimus bei juos savo nuožiūra šalinti.

### Funkciniai reikalavimai

#### Neregistruotas/neprisijungęs sistemos naudotojas galės:
 1.	Peržiūrėti platformos reprezentacinį puslapį;
 2.	Prisiregistruoti ir/arba prisijungti prie internetinės aplikacijos.

#### Registruotas sistemos naudotojas galės:
 1.	Atsijungti nuo internetinės aplikacijos;
 2.	Prisijungti (užsiregistruoti) prie platformos;
 3.	Peržiūrėti atsiliepimus;
 4.	Pasirinktą atsiliepimą įvertinti;
 5.	Rašyti savo atsiliepimą;
 6.	Matyti visus savo atsiliepimus – pasirinktus pašalinti arba atnaujinti pakeitus tam tikrus duomenis;
 7.	Naudotis atsiliepimų paieška;
 8.	Matyti bendrus savo visų atsiliepimų duomenis – susumuotus teigiamus ir neigiamus įvertinimus, bendrą parašytą atsiliepimų skaičių.

#### Administratorius galės:
 1.	Matyti visus vartotojus;
 2.	Matyti visus atsiliepimus;
 3.	Matyti konkretaus vartotojo atsiliepimus;
 4.	Naudotis atsiliepimų paieška;
 5.	Pašalinti vartotoją arba suteikti jam administratoriaus teises;
 6.	Pašalinti atsiliepimą;

## Sistemos architektūra
Sistemos sudedamosios dalys:

•	Kliento pusė (angl. Front-End) – naudojant Vue.js;

•	Serverio pusė (angl. Back-End) – naudojant C# .NET. Duomenų bazė – MSSQL.


Pav. 1 pavaizduota kuriamos sistemos diegimo diagrama. Sistemos talpinimui yra naudojamas Azure serveris. Kiekviena sistemos dalis yra diegiama tame pačiame serveryje. Internetinė aplikacija yra pasiekiama per HTTP protokolą. Šios sistemos veikimui (pvz., duomenų manipuliavimui su duomenų baze) yra reikalingas GoAuto API.

![image](https://user-images.githubusercontent.com/79079004/190923953-9fe4ce91-234b-43f9-9436-2eec6484dedb.png)


<div align="center">Pav. 1 Sistemos GoAuto diegimo diagrama</div>

## API specifikacija

### Automobilių metodai





