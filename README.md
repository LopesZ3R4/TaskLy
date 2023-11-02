Backend do Projeto de Calendário de Tarefas
Este é o backend do projeto de calendário de tarefas, que é um desafio para um processo seletivo de uma empresa. Ele usa ASP.NET Core + Entity Framework Core e SQL Server para o banco de dados. Ele fornece uma API RESTful para interagir com o frontend e realizar operações CRUD nas tarefas do dia.

Funcionalidades
O backend implementa as seguintes funcionalidades, de acordo com o nível de senioridade desejado:

Júnior: Cadastro, edição e remoção de tarefas do dia
Pleno: Busca de tarefas por título ou tags1
Sênior: Consulta de feriados nacionais, login e dashboard
Requisitos
.NET 6
SQL Server 2022
Visual Studio 2022 ou VS Code
Instalação
Clone este repositório usando git clone [1](https://github.com/TaskLy.git)
Abra a solução backend.sln no Visual Studio ou VS Code
Altere a string de conexão do banco de dados no arquivo appsettings.json
Execute o comando dotnet ef database update para criar o banco de dados e as tabelas
Execute o projeto usando dotnet run ou pressionando F5
Uso
O backend expõe os seguintes endpoints:

GET /api/tarefas - Retorna todas as tarefas do dia
GET /api/tarefas/{id} - Retorna uma tarefa específica pelo id
POST /api/tarefas - Cria uma nova tarefa
PUT /api/tarefas/{id} - Atualiza uma tarefa existente pelo id
DELETE /api/tarefas/{id} - Deleta uma tarefa existente pelo id
GET /api/tarefas/buscar?titulo={titulo}&tags={tags} - Busca tarefas por título ou tags
GET /api/feriados - Retorna os feriados nacionais do ano atual
POST /api/login - Autentica um usuário e retorna um token JWT
GET /api/dashboard - Retorna algumas estatísticas sobre as tarefas do usuário (requer autenticação)