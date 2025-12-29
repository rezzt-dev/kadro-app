#  KADRO
Local task and board manager with a modern interface, developed in WPF and C#.

---
###   GENERAL DESCRIPTION
**Kadro** is a local desktop application designed for managing and organising boards and tasks. It enables users to structure their workflow through configurable Kanban-style panels, providing a clear and adaptable visual overview tailored to individual requirements.

Key features include:
- Creation of multiple boards with custom names and colours.
- Panel management within each board, including the default ones: Pending, In Progress, and Completed.
- Creation, editing, and organisation of tasks.
- Local data storage using SQLite to ensure independence from network connectivity and provide high performance.
- A modern and minimalist interface developed entirely with XAML.

---
###   TECHNOLOGIES USED
| Categoría                                | Tecnologías |
| ---------------------------------------- | ----------- |
| Lenguajes                                | C#          |
| Framework principal                      | .NET WPF    |
| Framework alternativo / expansión futura | Avalonia    |
| Almacenamiento                           | SQLite      |
| Interfaz                                 | XAML        |
| Control de versiones                     | Git         |

---
###   SYSTEM STRUCTURE AND DATA MODEL
Kadro employs a clear and extensible structure based on four main entities: Board, Panel, Task, and Status.

####     1. BOARD
Represents a board that contains panels.
| Field             | Type     | Description            |
| ----------------- | -------- | ---------------------- |
| board_id          | int      | Board identifier       |
| name              | string   | Assigned name          |
| color             | string   | Representative colour  |
| creation_date     | datetime | Creation date          |
| modification_date | datetime | Last modification date |

####     2. PANEL
Represents a panel within a board.
| Field         | Type                            |
| ------------- | ------------------------------- |
| panel_id      | int                             |
| board_id      | int                             |
| name          | string                          |
| color         | string                          |
| order         | int (position within the board) |
| creation_date | datetime                        |

####     3. TASK
Represents a task within a panel.
| Field         | Type     |
| ------------- | -------- |
| task_id       | int      |
| panel_id      | int      |
| title         | string   |
| description   | string   |
| tag           | string   |
| creation_date | datetime |
| end_date      | datetime |
| priority      | int      |
| status_id     | int      |

####     4. STATUS
Defines the state of a task.
| Field     | Type   |
| --------- | ------ |
| status_id | int    |
| name      | string |

---
###   INSTALLATION AND EXECUTION
####     1. CLONACION DEL REPOSITORIO
```sh
git clone https://github.com/usuario/Kadro.git
cd Kadro
```
