# Identity Map Pattern
   
## Contexto
<p>O Identity Map é um pattern que visa trabalhar somente com uma única versão de um registro com o objetivo de não gerar conflitos durante o fluxo de um processamento.</p>
<p>O seu conceito consiste em: </p>
<ul>
  <li>A primeira solicitação de busca por um registro, irá acessar diretamte a fonte de dados e salvar a sua versão em memória.</li>
  <li>As solicitações posteriores, durante o processamento daquele fluxo, irão acessar o registro que está em memória pela primeira solicitação feita.</li>
</ul>
<p>Um exemplo de utilização desse pattern é implementá-lo na camada de Data/Repository da sua aplicação.</p>
<p>Com isso, quando for solicitado a busca de um registro (seja qual for a fonte de dados), a primeira solicitação irá pesquisar diretamente na origem de dados (um Banco de Dados ou um Cache, por exemplo). Enquanto isso, as demais solicitações daquele fluxo irão utilizar deste registro já retornado pela primeira solicitação.</p>
<p>Realizando essa implementação na camada de Data, é observado alguns benefícios, como: </p>
<ul>
  <li>O acesso ao banco de dados irá diminuir.</li>
  <li>
    <p>Não necessariamente precisamos repassar o objeto como parâmetro durante todo o fluxo do processamento, pois pode ser utilizado a interface de acesso a dados normalmente visto que a busca no banco de dados irá ocorrer uma única vez.</p>
    <p>Com isso, a necessidade de parâmetros tende a diminuir e os registros podem ser solicitados em qualquer ponto do processamento, sem aumentar as idas na fonte de dados.</p>
  </li>
  <li>Não será possível, no mesmo fluxo de processamento, trabalhar com mais de uma versão para o mesmo registro.</li>
</ul>

## Exemplo de utilização
<p>Neste exemplo, foi criado apenas 3 camadas: </p>
<ul>
  <li>Domain</li>
  <li>Data</li>
  <li>Apresentação</li>
</ul>
<p>Na camada de Data, é encontrado a implementação do Context do EF e o Repositories. Mas, lém disso, foi implementado a utilização do Identity Map.</p>
<p>A implementação consiste no seguinte fluxo:</p>
<ol>
  <li>
    É necessário a criação de uma chave única para identificar o registro no Identity Map. O padrão utilizado foi o nome da entidade, o nome do método de busca e algum parâmetro. <a href="https://github.com/martineli17/patterns-identity-map/blob/master/Data/Repositories/ClientRepository.cs">Arquivo</a>
     
      public async Task<IEnumerable<Client>> GetByNameAsync(string name, CancellationToken cancellationToken)
        {
            var key = $"{nameof(Client)}_{nameof(GetByNameAsync)}_{name}";
            return await _identityMap.ExecuteAsync(key, () => _databaseContext.Client.Where(x => x.Name.ToLower().Contains(name)).ToListAsync(cancellationToken));
        }
  </li>
  <li>
    Ao solicitar o o registro através do Identity Map, é verificado se a chave informada existe. Se existir, retorna o registro que está em memória. Caso não exista, realiza a busca normalmente e salva o retorno com a chave informada. <a href="https://github.com/martineli17/patterns-identity-map/blob/master/Data/Base/IdentityMapQueryCommand.cs">Arquivo</a>
    
    public async Task<TReturn> ExecuteAsync<TReturn>(string key, Func<Task<TReturn>> func)
        {
            if (_identityMap.HasObject(key))
            {
                _logger.LogInformation("Getting the data {0} from Identity Map", key);
                return _identityMap.GetObjet<TReturn>(key);
            }

            var result = await func();
            _logger.LogInformation("Getting the data {0} from DataBase", key);
            _identityMap.MapObject(key, result);

            return result;
        }

  </li>
  <li>
    E a implementação do Identity Map, fica da seguinte maneira: <a href="https://github.com/martineli17/patterns-identity-map/blob/master/Data/Base/IdentityMapManager.cs">Arquivo</a>

    public class IdentityMapManager : IIdentityMapManager
    {
        private readonly Dictionary<string, object> _objectsMapped;

        public IdentityMapManager()
        {
            _objectsMapped = new();
        }

        public TReturn GetObjet<TReturn>(string key)
        {
            return (TReturn)_objectsMapped[key];
        }

        public void MapObject(string key, object value)
        {
            if (!_objectsMapped.ContainsKey(key))
                _objectsMapped[key] = value;
        }

        public bool HasObject(string key)
        {
            return _objectsMapped.ContainsKey(key);
        }
    }
  </li>
  <li>
    No final, quem solicitar estes dados não precisa se preocupar em estar acessando o banco de dados em cada uma das chamadas (código somente para testes):

    [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            await _clientRepository.GetAllAsync(cancellationToken);
            var result = await _clientRepository.GetAllAsync(cancellationToken);

            return Ok(result);
        }
  </li>
</ol>

