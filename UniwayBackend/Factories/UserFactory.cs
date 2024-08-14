namespace UniwayBackend.Factories
{
    public class UserFactory
    {
        private readonly IReadOnlyDictionary<int, IUser> _userMap; // Guardará las instancias de TechnicalCreator o ClientCreator

        public UserFactory(IEnumerable<IUser> users)
        {
            // Se inicializa con los la llave del Role de la instancia y el objeto
            _userMap = users.ToDictionary(user => user.GetRoleId(), user => user);
        }

        /// <summary>
        /// Retorna la instancia de un Creator en especifico dependiendo del RolId
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public IUser GetUser(int RoleId)
        {
            if (_userMap.TryGetValue(RoleId, out var user))
            {
                return user;
            }
            throw new ArgumentException($"El usuario con rol {RoleId} no es soportado");
        }
    }
}
