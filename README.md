## Executando o Projeto

* Abra o projeto no Visual studio
* No gerenciador de soluções clice com o botão direito sobre o projeto ```ReceitaWs.WebApi``` e defina-o como projeto de inicialização.
* Clique na setinha apontando para baixo ao lado do botão de executar e selecione o Perfil ```ReceitaWs.WebApi```.
* Agora o projeto está pronto para ser executado.

## Executando o Container do SqlServer *"Windows"*

* Abra um terminal na pasta raiz do projeto e rode o seguinte comando:
* ```docker run --cap-add SYS_PTRACE -e "ACCEPT_EULA=1" -e "MSSQL_SA_PASSWORD=Strong@pass" -p 1433:1433 --name mssql-receitaWs -d mcr.microsoft.com/azure-sql-edge```
* Após o comando terminar de ser executado seu container estará pronto para uso.

## Migrations
* Abra um terminal na pasta raiz do projeto e rode o comando seguinte para certificar que o EntityFramework está atualizado.
* ```dotnet tool update --global dotnet-ef```
* Após certificar-se que o EF está atualizado só executar o migrations com o seguinte comando:
* ```dotnet ef database update --project ReceitaWsExtractor.Infra --connection "Server=localhost;Database=mssql-receitaWs;User Id=sa;Password=Strong@pass;TrustServerCertificate=True"```
* Depois que o comando terminar de ser executado o seu canto de dados terá sido criado com sucesso!
