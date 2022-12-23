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

# API specifikacija

## Automobilių metodai

### GET API/Cars

Metodas yra prieinamas tik administratoriams ir grąžina visus sukurtus automobilius

#### Metodo URL

`https://localhost:7221/API/Cars`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| OK           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Not found    | 404   |

#### Užklausos pavyzdys

`GET https://localhost:7221/API/Cars`

#### Atsako pavyzdys

```
[
    {
        "id": 1,
        "brand": "Audi",
        "model": "A4",
        "generation": "B7",
        "startYear": "2004",
        "endYear": "2008",
        "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
    },
    {
        "id": 2,
        "brand": "Audi",
        "model": "A4",
        "generation": "B8",
        "startYear": "2007",
        "endYear": "2015",
        "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
    }
]
```
### GET API/Cars/{id}

Metodas yra prieinamas tik administratoriams ir grąžina automobilį pagal id

#### Metodo URL

`https://localhost:7221/API/Cars/{id}`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| OK           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Not found    | 404   |

#### Užklausos pavyzdys

`GET https://localhost:7221/API/Cars/1`

#### Atsako pavyzdys

```
{
  "id": 1,
  "brand": "Audi",
  "model": "A4",
  "generation": "B7",
  "startYear": "2004",
  "endYear": "2008",
  "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
}

```
### PUT API/Cars/{id}

Metodas yra prieinamas tik administratoriams ir atnaujina automobilį pagal id

#### Metodo URL

`https://localhost:7221/API/Cars/{id}`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| No content   | 204   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |

#### Parametrai

| Pavadinimas  | Ar būtinas? |    Apibūdinimas                | Pavyzdys |
| ------------ |-------------|--------------------------------|----------|
| brand        | Taip        | Automobilio markė              | Audi     |
| model        | Taip        | Automobilio modelis            | A6       |
| generation   | Taip        | Automobilio karta              | C6       |
| startYear    | Taip        | Modelio gamybos pradžios metai | 2004     |
| endYear      | Taip        | Modelio gamybos pabaigos metai | 2011     |


#### Užklausos pavyzdys

`PUT https://localhost:7221/API/Cars/1`

```
{
  "brand": "Audi",
  "model": "A4",
  "generation": "B7",
  "startYear": "2004",
  "endYear": "2009"
}

```

#### Atsako pavyzdys

```
Atsako kodas 204

```

### GET API/Cars/{id}

Metodas yra prieinamas tik administratoriams ir grąžina automobilį pagal id

#### Metodo URL

`https://localhost:7221/API/Cars/{id}`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| OK           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Not found    | 404   |

#### Užklausos pavyzdys

`GET https://localhost:7221/API/Cars/1`

#### Atsako pavyzdys

```
{
  "id": 1,
  "brand": "Audi",
  "model": "A4",
  "generation": "B7",
  "startYear": "2004",
  "endYear": "2008",
  "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
}

```
### POST API/Cars

Metodas yra prieinamas tik administratoriams ir sukuria naują automobilį

#### Metodo URL

`https://localhost:7221/API/Cars/`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| Created      | 201   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |

#### Parametrai

| Pavadinimas  | Ar būtinas? |    Apibūdinimas                | Pavyzdys |
| ------------ |-------------|--------------------------------|----------|
| brand        | Taip        | Automobilio markė              | Audi     |
| model        | Taip        | Automobilio modelis            | A6       |
| generation   | Taip        | Automobilio karta              | C6       |
| startYear    | Taip        | Modelio gamybos pradžios metai | 2004     |
| endYear      | Taip        | Modelio gamybos pabaigos metai | 2011     |


#### Užklausos pavyzdys

`POST https://localhost:7221/API/Cars/`

```
{
  "brand": "Audi",
  "model": "A4",
  "generation": "B7",
  "startYear": "2004",
  "endYear": "2009"
}

```

#### Atsako pavyzdys

```
{
  "id": 10,
  "brand": "Audi",
  "model": "A4",
  "generation": "B7",
  "startYear": "2004",
  "endYear": "2009",
  "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
}

```

### DELETE API/Cars/{id}

Metodas yra prieinamas tik administratoriams ir ištrina automobilį pagal id

#### Metodo URL

`https://localhost:7221/API/Cars/{id}`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| No content   | 204   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |

#### Užklausos pavyzdys

`DELETE https://localhost:7221/API/Cars/1`

#### Atsako pavyzdys

```
Atsako kodas 204

```

### GET API/Cars/{brand}/FilterByBrand

Metodas yra prieinamas tik administratoriams ir išfiltruoja automobilius pagal markę

#### Metodo URL

`https://localhost:7221/API/Cars/{brand}/FilterByBrand`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| Ok           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |

#### Užklausos pavyzdys

`GET https://localhost:7221/API/Cars/Audi/FilterByBrand`

#### Atsako pavyzdys

```
[
  {
    "id": 3,
    "brand": "Audi",
    "model": "A4",
    "generation": "B8",
    "startYear": "2007",
    "endYear": "2015",
    "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
  },
  {
    "id": 4,
    "brand": "Audi",
    "model": "A4",
    "generation": "B9",
    "startYear": "2006",
    "endYear": "2015",
    "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
  },
  {
    "id": 10,
    "brand": "Audi",
    "model": "A3",
    "generation": "8P",
    "startYear": "2004",
    "endYear": "2010",
    "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
  }
]

```

### POST API/Cars/FilterByModel

Metodas yra prieinamas tik administratoriams ir išfiltruoja automobilius pagal modelį

#### Metodo URL

`https://localhost:7221/API/Cars/FilterByModel`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| OK           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |

#### Parametrai

| Pavadinimas  | Ar būtinas? |    Apibūdinimas                | Pavyzdys                             |
| ------------ |-------------|--------------------------------|--------------------------------------|
| brand        | Taip        | Automobilio markė              | Audi                                 |
| model        | Taip        | Automobilio modelis            | A6                                   |
| generation   | Taip        | Automobilio karta              | C6                                   |
| startYear    | Taip        | Modelio gamybos pradžios metai | 2004                                 |
| endYear      | Taip        | Modelio gamybos pabaigos metai | 2011                                 |
| userId       | Taip        | Sukūrusio naudotojo id         | 1012f8e2-fe89-45fe-92d6-678730085ece |


#### Užklausos pavyzdys

`POST https://localhost:7221/API/Cars/FilterByModel`

```
{
  "model": "A4",
  "filteredCars": [
     {
     "id": 3,
     "brand": "Audi",
     "model": "A4",
     "generation": "B8",
     "startYear": "2007",
     "endYear": "2015",
     "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
   },
   {
     "id": 4,
     "brand": "Audi",
     "model": "A4",
     "generation": "B9",
     "startYear": "2006",
     "endYear": "2015",
     "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
   },
   {
     "id": 10,
     "brand": "Audi",
     "model": "A3",
     "generation": "8P",
     "startYear": "2004",
     "endYear": "2010",
     "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
   }
  ]
}

```

#### Atsako pavyzdys

```
[
  {
    "id": 3,
    "brand": "Audi",
    "model": "A4",
    "generation": "B8",
    "startYear": "2007",
    "endYear": "2015",
    "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
  },
  {
    "id": 4,
    "brand": "Audi",
    "model": "A4",
    "generation": "B9",
    "startYear": "2006",
    "endYear": "2015",
    "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
  }
]

```

### POST API/Cars/GetIdByGeneration

Metodas yra prieinamas tik administratoriams ir grąžina automobilio id pagal kartą

#### Metodo URL

`https://localhost:7221/API/Cars/GetIdByGeneration`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| OK           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |

#### Parametrai

| Pavadinimas  | Ar būtinas? |    Apibūdinimas                | Pavyzdys                             |
| ------------ |-------------|--------------------------------|--------------------------------------|
| brand        | Taip        | Automobilio markė              | Audi                                 |
| model        | Taip        | Automobilio modelis            | A6                                   |
| generation   | Taip        | Automobilio karta              | C6                                   |
| startYear    | Taip        | Modelio gamybos pradžios metai | 2004                                 |
| endYear      | Taip        | Modelio gamybos pabaigos metai | 2011                                 |
| userId       | Taip        | Sukūrusio naudotojo id         | 1012f8e2-fe89-45fe-92d6-678730085ece |


#### Užklausos pavyzdys

`POST https://localhost:7221/API/Cars/GetIdByGeneration`

```
{
  "generation": "B8",
  "filteredCars": [
     {
     "id": 3,
     "brand": "Audi",
     "model": "A4",
     "generation": "B8",
     "startYear": "2007",
     "endYear": "2015",
     "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
   },
   {
     "id": 4,
     "brand": "Audi",
     "model": "A4",
     "generation": "B9",
     "startYear": "2006",
     "endYear": "2015",
     "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
   }
  ]
}

```

#### Atsako pavyzdys

```
3

```

## Reakcijų į atsiliepimus metodai

### POST API/Responses/

Metodas yra prieinamas tik registruotiems vartotojams ir įrašo atsiliepimo įvertinima, bei pakeičia įvertinimo vertę atsiliepime

#### Metodo URL

`https://localhost:7221/API/Responses`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| Created      | 201   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |

#### Parametrai

| Pavadinimas  | Ar būtinas? |  Apibūdinimas        | Pavyzdys   |
| ------------ |-------------|----------------------|------------|
| brand        | Taip        | Automobilio markė    | Audi       |
| model        | Taip        | Automobilio modelis  | A6         |



#### Užklausos pavyzdys

`POST https://localhost:7221/API/Responses`

```
{
  "fkReviewId": 0,
  "status": 1
}

```

#### Atsako pavyzdys

```
Atsako kodas 201

```

### GET API/Responses/

Metodas yra prieinamas tik registruotiems vartotojams ir grąžina visus atsiliepimų įvertinimus

#### Metodo URL

`https://localhost:7221/API/Responses`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| OK           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |


#### Užklausos pavyzdys

`GET https://localhost:7221/API/Responses`

#### Atsako pavyzdys

```
[
 {
  "id": 1
  "fkReviewId": 1,
  "status": 1,
  "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
 },
 {
  "id": 2
  "fkReviewId": 3,
  "status": 0,
  "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
 },
]

```
## Atsiliepimų metodai

### POST API/Reviews

Metodas yra prieinamas tik registuotiems vartotojams ir sukuria naują atsiliepimą

#### Metodo URL

`https://localhost:7221/API/Reviews`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| Created      | 201   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |

#### Parametrai

| Pavadinimas        | Ar būtinas? |    Apibūdinimas       | Pavyzdys   |
| -------------------|-------------|-----------------------|------------|
| brand              | Taip        | Automobilio markė     | Audi       |
| model              | Taip        | Automobilio modelis   | A6         |
| generation         | Taip        | Automobilio karta     | C6         |
| text               | Taip        | Atsiliepimo tekstas   | Geras auto |
| engineDisplacement | Taip        | Variklio tūris        | 2011       |
| enginePower        | Taip        | Variklio galia        | 2011       |
| positives          | Taip        | Pranašumai            | 2011       |
| negatives          | Taip        | Trūkumai              | 2011       |
| finalScore         | Taip        | Galutinis įvertinimas | 2011       |


#### Užklausos pavyzdys

`POST https://localhost:7221/API/Reviews`

```
{
  "text": "Geras auto",
  "engineDisplacement": 3,
  "enginePower": 200,
  "positives": "Gera kokybė",
  "negatives": "Brangus remontas",
  "finalScore": 8,
  "brand": "Audi",
  "model": "A4",
  "generation": "B8"
}

```

#### Atsako pavyzdys

```
{
  "id": 14,
  "text": "Geras auto",
  "creationDate": "2022-12-23",
  "engineDisplacement": 3,
  "enginePower": 200,
  "likes": 0,
  "dislikes": 0,
  "positives": "Gera kokybė",
  "negatives": "Brangus remontas",
  "finalScore": 8,
  "username": "admin",
  "brand": "Audi",
  "model": "A4",
  "generation": "B8",
  "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
}

```

### GET API/Reviews/{id}/GetByCarId

Metodas yra prieinamas tik registuotiems vartotojams ir grąžina visus atsiliepimus pagal automobilio id

#### Metodo URL

`https://localhost:7221/API/Reviews/{id}/GetByCarId`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| OK           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |

#### Užklausos pavyzdys

`GET https://localhost:7221/API/Reviews/3/GetByCarId`


#### Atsako pavyzdys

```
[
  {
    "id": 14,
    "text": "Geras auto",
    "creationDate": "2022-12-23T20:17:35.537",
    "engineDisplacement": 3,
    "enginePower": 200,
    "likes": 0,
    "dislikes": 0,
    "positives": "Gera kokybe",
    "negatives": "Brangus remontas",
    "finalScore": 8,
    "userId": "1012f8e2-fe89-45fe-92d6-678730085ece",
    "fkCarId": 3
  }
]

```

### GET API/Reviews/{userName}/review

Metodas yra prieinamas tik registuotiems vartotojams ir grąžina visus atsiliepimus pagal vartotojo vardą

#### Metodo URL

`https://localhost:7221/API/Reviews/{userName}/review`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| OK           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |

#### Užklausos pavyzdys

`GET https://localhost:7221/API/Reviews/admin/review`


#### Atsako pavyzdys

```
[
  {
    "id": 14,
    "text": "Geras auto",
    "creationDate": "2022-12-23",
    "engineDisplacement": 3,
    "enginePower": 200,
    "likes": 0,
    "dislikes": 0,
    "positives": "Gera kokybe",
    "negatives": "Brangus remontas",
    "finalScore": 8,
    "username": "admin",
    "brand": "Audi",
    "model": "A4",
    "generation": "B8",
    "userId": "1012f8e2-fe89-45fe-92d6-678730085ece"
  }
]

```

### GET API/Reviews/{id}

Metodas yra prieinamas tik registuotiems vartotojams ir grąžina atsiliepimą pagal id

#### Metodo URL

`https://localhost:7221/API/Reviews/{id}`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| OK           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |

#### Užklausos pavyzdys

`GET https://localhost:7221/API/Reviews/14`


#### Atsako pavyzdys

```
{
  "id": 14,
  "text": "Geras auto",
  "creationDate": "2022-12-23T20:17:35.537",
  "engineDisplacement": 3,
  "enginePower": 200,
  "likes": 0,
  "dislikes": 0,
  "positives": "Gera kokybe",
  "negatives": "Brangus remontas",
  "finalScore": 8,
  "userId": "1012f8e2-fe89-45fe-92d6-678730085ece",
  "fkCarId": 3
}

```

### PUT API/Reviews/{id}

Metodas yra prieinamas tik registuotiems vartotojams ir atnaujina atsiliepimą

#### Metodo URL

`https://localhost:7221/API/Reviews/{id}`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| No content   | 204   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Bad request  | 400   |
| Not found    | 404   |

#### Parametrai

| Pavadinimas        | Ar būtinas? |    Apibūdinimas       | Pavyzdys   |
| -------------------|-------------|-----------------------|------------|
| brand              | Taip        | Automobilio markė     | Audi       |
| model              | Taip        | Automobilio modelis   | A6         |
| generation         | Taip        | Automobilio karta     | C6         |
| text               | Taip        | Atsiliepimo tekstas   | Geras auto |
| engineDisplacement | Taip        | Variklio tūris        | 2011       |
| enginePower        | Taip        | Variklio galia        | 2011       |
| positives          | Taip        | Pranašumai            | 2011       |
| negatives          | Taip        | Trūkumai              | 2011       |
| finalScore         | Taip        | Galutinis įvertinimas | 2011       |


#### Užklausos pavyzdys

`PUT https://localhost:7221/API/Reviews/14`

```
{
  "text": "Neblogas automobilis viskam",
  "engineDisplacement": 3,
  "enginePower": 150,
  "positives": "Gera kokybė",
  "negatives": "Brangus remontas",
  "finalScore": 7,
  "brand": "Audi",
  "model": "A4",
  "generation": "B8"
}

```

#### Atsako pavyzdys

```
Atsako kodas 204

```

### DELETE API/Reviews/{id}

Metodas yra prieinamas tik registuotiems vartotojams ir ištrina atsiliepimą pagal id

#### Metodo URL

`https://localhost:7221/API/Reviews/{id}`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| No content   | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Not found    | 404   |

#### Užklausos pavyzdys

`DELETE https://localhost:7221/API/Reviews/14`

#### Atsako pavyzdys

```
Atsako kodas 204

```

### GET API/Reviews/GetResponsesResult

Metodas yra prieinamas tik registuotiems vartotojams ir grąžina atsiliepimo teigiamų bei neigiamų įvertinimų sumas

#### Metodo URL

`https://localhost:7221/API/Reviews/GetResponsesResult`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| OK           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Not found    | 404   |

#### Užklausos pavyzdys

`GET https://localhost:7221/API/Reviews/GetResponsesResult`


#### Atsako pavyzdys

```
{
  "likes": 5,
  "dislikes": 2
}

```

### GET API/Reviews/{userId}/GetReviewCount

Metodas yra prieinamas tik registuotiems vartotojams ir grąžina vartotojo parašytų atsiliepimų skaičių

#### Metodo URL

`https://localhost:7221/API/Reviews/{userId}/GetReviewCount`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| OK           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Not found    | 404   |
| Bad request  | 400   |


#### Užklausos pavyzdys

`GET https://localhost:7221/API/Reviews/1012f8e2-fe89-45fe-92d6-678730085ece/GetReviewCount`

#### Atsako pavyzdys

```
5

```

### GET API/Reviews/{userId}/GetByUserId

Metodas yra prieinamas tik registuotiems vartotojams ir grąžina atsiliepimus pagal parašusio vartotojo vardą

#### Metodo URL

`https://localhost:7221/API/Reviews/{userId}/GetByUserId`

#### Atsakymų kodai

| Pavadinimas  | Kodas |
| ------------ |-------|
| OK           | 200   |
| Unauthorized | 401   |
| Forbidden    | 403   |
| Not found    | 404   |
| Bad request  | 400   |


#### Užklausos pavyzdys

`GET https://localhost:7221/API/Reviews/1012f8e2-fe89-45fe-92d6-678730085ece/GetByUserId`

#### Atsako pavyzdys

```
[
  {
    "id": 14,
    "text": "Neblogas automobilis viskam",
    "creationDate": "2022-12-23T20:17:35.537",
    "engineDisplacement": 3,
    "enginePower": 150,
    "likes": 0,
    "dislikes": 0,
    "positives": "Gera kokybe",
    "negatives": "Brangus remontas",
    "finalScore": 7,
    "userId": "1012f8e2-fe89-45fe-92d6-678730085ece",
    "fkCarId": 3
  }
]

```






























