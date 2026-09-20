# HBase Student Management System

A distributed student management application built with **C# and .NET 8 Windows Forms**, using the **Apache HBase REST API** as the data layer.

This project was developed as part of the **Distributed Computing Systems** course at the University of Information Technology, VNU-HCM.

## Team Members

| Member | Main Contributions | Contribution |
|---|---|---:|
| **Doan Ngoc Minh Chau** | HBase system analysis and design, cluster deployment and operation, system evaluation, presentation slides, and demo video | 33.3% |
| **Le Nhat Trinh Nguyen** | Introduction, system evaluation, report review and editing, presentation slides, and demo video | 33.3% |
| **Vo Thi Hong Phuc** | Theoretical background, conclusion and future work, and presentation slides | 33.3% |

This project was completed collaboratively as a three-member academic team.

## Overview

The project demonstrates how a desktop application can interact with a distributed Apache HBase cluster through REST APIs.

The original system was deployed on a **3-node Ubuntu Server cluster** using VMware Workstation:

- **1 Master Node**
  - HMaster
  - Cluster coordination and monitoring
  - ZooKeeper
  - Web management interface

- **2 Worker Nodes**
  - HRegionServer
  - DataNode
  - NodeManager
  - ZooKeeper

The cluster was configured in **Fully Distributed Mode** with HDFS for distributed storage and ZooKeeper for coordination.

## Architecture

```text
C# WinForms Application
        |
        | HTTP / REST API
        v
HBase REST Server
        |
        v
Apache HBase Cluster
        |
        +-- HMaster
        |
        +-- RegionServer 01
        |
        +-- RegionServer 02
        |
        v
       HDFS
```

## Technologies

### Application
- C#
- .NET 8
- Windows Forms
- REST API
- HttpClient

### Distributed System
- Apache HBase 1.3.5
- Hadoop HDFS
- Hadoop YARN
- Apache ZooKeeper
- Ubuntu Server 20.04.6
- VMware Workstation

## Features

- View student records
- Add new students
- Update student information
- Delete students
- Delete all records
- Import student data from CSV
- Export student data to CSV
- Download CSV template
- Display operation progress and execution time

## Data Flow

```text
WinForms Application
        |
        v
REST Client
        |
        v
HBase REST Server
        |
        v
HMaster / RegionServer
        |
        v
HDFS
```

## Performance Evaluation

The application was tested with different dataset sizes.

| Dataset Size | Import Time | Processing Speed |
|---|---:|---:|
| 5,000 records | 21.29 s | ~235 rows/s |
| 10,000 records | 42.62 s | ~235 rows/s |
| 15,000 records | 52.00 s | ~288 rows/s |

## Fault-Tolerance Test

A RegionServer was manually stopped during testing to evaluate system availability.

Observed behavior:

- HMaster detected the unavailable RegionServer.
- Regions were reassigned to the remaining RegionServer.
- The `students` table remained available.
- The application continued to perform Load, Add, Update, and Delete operations.
- No data loss was observed during the test.
- After restarting the RegionServer, it successfully rejoined the cluster.

## Configuration

Before running the application, update the HBase REST Server address in `HBaseManager.cs`.

Example:

```csharp
private readonly string baseUrl =
    "http://YOUR_HBASE_SERVER_IP:8090/students";
```

Make sure the HBase REST Server is running before starting the application.

## Demo

🎥 **Video Demo:**  
https://drive.google.com/drive/folders/1Co4G50KezhaD4e-bWFT0C4n6oORf_3Vx

## Project Structure

```text
HBaseStudentApp/
│
├── HBaseStudentApp.sln
├── .gitignore
│
└── HBaseStudentApp/
    ├── HBaseStudentApp.csproj
    ├── Program.cs
    ├── MainForm.cs
    ├── MainForm.Designer.cs
    ├── MainForm.resx
    ├── HBaseManager.cs
    ├── Student.cs
    ├── CellModel.cs
    └── Properties/
```

## Academic Project

**Course:** Distributed Computing Systems  
**University:** University of Information Technology – VNU-HCM

The project focuses on deploying and evaluating a distributed Apache HBase environment and building a practical client application to interact with the cluster.
