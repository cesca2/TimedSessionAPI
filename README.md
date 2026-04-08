# Timed Session API

C# .NET web API designed to log occurences of an event and duration to monitor a "timed session". Implements CRUD operations.

## Demo

![Demo Animation](../demo/api.gif?raw=true)

## Features

* Record timed sessions in SQLite database with the following details:
    * Event type
    * Date
    * Duration
* GET, POST, DELETE, PUT endpoints
* Pagination of data using LastDate parameter for filtering 

## Pre-requsites 

### Dependencies 

* .NET 10.0 installation


## Run Locally

Clone the project

```bash
  git clone git@github.com:cesca2/TimedSessionAPI.git
```

Go to the project directory

```bash
  cd TimedSessionAPI
```

Run the application

```bash
  dotnet run
```


## API Reference

### Get items

```http
  GET /api/Sessions
```
| Query Parameters | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `LastDate`      | `string` | **Optional**. Date for session filtering in format YYYY-MM-DD  |

### Create item

```http
  POST /api/Sessions
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **Required**. Id of item to fetch |

EXAMPLE INPUT:
```json
{
  "type": "C#",
  "date": "2026-03-27",
  "start": "11:00",
  "end": "12:30"
}
```
| Field      | Type    | Required | Description                           |
| ---------- | ------- | -------- | ----------------------------------    |
| `type`     | string  | Yes      | Keyword to describe the session type  |
| `date`    | string  | Yes      | Date of session in format YYYY-MM-DD  |
| `start` | string  | Yes      | Start time of session in format HH:MM |
| `end`    | string  | Yes      | End time of session in format HH:MM   |


### Update item

```http
  PUT /api/Sessions/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **Required**. Id of item to update|

EXAMPLE INPUT:
```json
{
  "type": "C#",
  "date": "2026-03-27",
  "start": "11:00",
  "end": "12:30"
}
```
| Field      | Type    | Required | Description                           |
| ---------- | ------- | -------- | ----------------------------------    |
| `type`     | `string`  | Yes      | Keyword to describe the session type  |
| `date`    | `string`  | Yes      | Date of session in format YYYY-MM-DD  |
| `start` | `string`  | Yes      | Start time of session in format HH:MM |
| `end`    | `string`  | Yes      | End time of session in format HH:MM   |


### Delete item

```http
  DELETE /api/Sessions/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **Required**. Id of item to update|


## Helpful Resources

 - Project inspiration from [Habit Logger](https://www.thecsharpacademy.com/project/12/habit-logger) and [Shifts Logger](https://www.thecsharpacademy.com/project/17/shifts-logger) projects
 - [Pagination tutorial](https://www.c-sharpcorner.com/article/implementing-pagination-and-filtering-in-asp-net-core-8-0-api/)
