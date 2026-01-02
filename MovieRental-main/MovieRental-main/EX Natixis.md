# Erro ao iniciar aplicação

Ao iniciar a aplicação deparamo-nos com o erro:
System.AggregateException: 'Some services are not able to be constructed (Error while validating the service descriptor 'ServiceType: MovieRental.Rental.IRentalFeatures Lifetime: Singleton ImplementationType: MovieRental.Rental.RentalFeatures': Cannot consume scoped service 'MovieRental.Data.MovieRentalDbContext' from singleton 'MovieRental.Rental.IRentalFeatures'.)'

Motivo:
RentalFeatures é adicionado como Singleton e depende de MovieDBContext que é injetado no construtor. Por defeito no EFCore ao adicionar um DBContext ele é adicionado como Scoped. Um Singleton não pode depender de um Scoped pelo que foi necessário alterar a declração do serviço RentalFeatures para Scoped.

# Método save async

A diferença entre sync e async é que o método async não bloqueia a thread enquanto outras operações estão a ser realizadas. No caso deste método save por exemplo, enquanto as operações de BD estão a ser realizadas a thread não fica bloqueada à espera e pode realizar outros trabalhos, melhorando a performance da aplicação.

# Tabela Customers

A tabela customers foi criada ao ser registada no DBContext e alterando as classes Customer e Rental para incluír Foreign Key de Customer. De notar que na classe Customer foi necessario adicionar um decorator [JsonIgnore] no atributo Rentals pois estava a criar um loop de serialização ao testar os métodos da aplicação. 
Não foi criado um Controller para Customer pelo que o método Save de Rental apenas funciona com CustomerID = 0. Testando com outros métodos ocorrem violações da Base de Dados.

# Método GetAll()

Para além da falta de async já referida no enunciado o método apresenta um erro transversal à aplicação toda ao devolver as entidades. Existe uma falta de DTOs transversal ao projeto visto apenas existir 1 modelo para cada agregado do domínio.
Por outro lado o método não tem qualquer safe guard contra problemas de tempo de execução. Por exenplo, se existirem muitos dados não está implementado qualquer tipo de paginação ou melhoria de desmepenho o que poderia levar a um tempo de execução inaceitável.

# Handling de exceções

Para handling de exceções da API inteira deveria-se implementar um Middleware que intercetasse as respostas dos pedidos devolvidas pela API (200, 404, etc.) e apresente os erros de forma uniforme. Devido ao pequeno tamanho poderia-se implementar blocos try catch em cada método, no entanto, tal não é boa prática pois leva a repetição de código e perda de valor na informação apresentada. Ou seja, embora os blocos try catch tenham valor em certos casos, como exceções que necessitam de ser tratadas de maneira não standard, iriam ser um entrave à escabilidade do projeto atual.


# Challenge

Para responder aos requisitos da Challenge começou-se por criar uma Interface PaymentProvider que define o atributo Paymentprovider (string) e o método pay. Alterou-se as classes já existentes para que implementem a interface e definam o atributo Paymentprovider (readonly) com o seu valor e de seguida foram registados no início da aplicação ficando disponíveis para serem injetadas na classe RentalFeatures.
Na RentalFeatures o  método Save foi atualizado para incluir um pagamento antes da criação do Rental na BD. O método procura o método de pagamento fornecido pelo objeto de criação do Rental pelos métodos existentes e, caso o encontre, processa o pagamento. Caso este método falhe a criação toda é abortada.
Para adicionar mais métodos de pagamento apenas é necessário criar a classe desejada e implementar a interface.


# Observações:

Ao tentar testar a aplicação a interface MovieFeatures não era registada na inicialização pelo que o controller de movies não a conseguia encontrar.