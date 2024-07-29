# 帧同步开发框架

这是一个基于netstandart2.0的帧同步游戏开发SDK，提供服务器，客户端部分。目前主要的代码示例用Unity编写客户端，netcore控制台程序为服务端。

## 项目结构如下

- `Docs/`
  - `Diagrams/`: 说明文件。  
  - `Protocols/`: 协议工具和配置。  
- `Engine/`: SDK目录。 
  - `Client/`: 供客户端项目使用的库，引用Common项目，基于netstandard2.0项目。
  - `Common/`: Client和Server项目的基础引用库，基于netstandard2.0项目。
  - `Server/`: 供服务端项目使用的库，引用Common项目，基于netstandard2.0项目。
- `Examples/`: 案例项目。
  - `Clients/`: 目前有一个案例项目，使用Unity开发。
  - `Servers/`: 目前一个netcore控制台程序。

## 说明
### API

```markdown  
@startuml  
class Animal {  
    +String name  
    +int age  
    +eat()  
    +sleep()  
}  

class Dog {  
    +bark()  
}  

class Cat {  
    +meow()  
}  

Animal <|-- Dog  
Animal <|-- Cat  
@enduml  