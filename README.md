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
  dotnet run --project TimedSessionAPI
```

Run the dedicated application integration tests

```bash
  dotnet test TimedSessionAPI.IntegrationTests
```

## API Reference

### Get items

```http
  GET /api/Sessions
```
| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **optional**. Id of item to fetch , equate to Guid|

| Query Parameters | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `LastDate`      | `string` | **Optional**. Date for session filtering in format YYYY-MM-DD  |

### Create item

```http
  POST /api/Sessions
```

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
| `date`    | string  | Yes      | Date of session in format DD/MM/YYYY  |
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
  "id": "7ab50dd5-c918-45ef-b1f2-99c24d6a7f24",
  "type": "C#",
  "date": "2026-03-27",
  "start": "11:00",
  "end": "12:30"
}
```
| Field      | Type    | Required | Description                           |
| ---------- | ------- | -------- | ----------------------------------    |
| `id`     | `string`  | Yes      | ID of session  |
| `type`     | `string`  | Yes      | Keyword to describe the session type  |
| `date`    | `string`  | Yes      | Date of session in format DD/MM/YYYY  |
| `start` | `string`  | Yes      | Start time of session in format HH:MM |
| `end`    | `string`  | Yes      | End time of session in format HH:MM   |


### Delete item

```http
  DELETE /api/Sessions/${id}
```

| Parameter | Type     | Description                       |
| :-------- | :------- | :-------------------------------- |
| `id`      | `string` | **Required**. Id of item to delete|


## Helpful Resources

 - Project inspiration from [Habit Logger](https://www.thecsharpacademy.com/project/12/habit-logger) and [Shifts Logger](https://www.thecsharpacademy.com/project/17/shifts-logger) projects
 - [Pagination tutorial](https://www.c-sharpcorner.com/article/implementing-pagination-and-filtering-in-asp-net-core-8-0-api/)
 - [Integration testing tutorial](https://learn.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-10.0&pivots=xunit), [Integration testing example 1](https://github.com/martincostello/dotnet-minimal-api-integration-testing/tree/main), [Integration testing example 2](https://medium.com/@ajaykumar1807/integration-testing-for-dotnet-core-apis-handling-database-925507b282b5)
