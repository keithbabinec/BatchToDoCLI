namespace BatchToDoCLI
{
    public enum ExitCodes
    {
        Success = 0,

        InvalidArguments = 1,

        MissingOrInvalidSettingsJson = 2,

        MissingOrInvalidDefinitionYaml = 3,

        FailedToTransformDefinitionYaml = 4,

        FailedToAuthenticate = 5
    }
}
