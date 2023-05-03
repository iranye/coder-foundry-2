namespace MediaManagerTests;

using MediaManager.WPF.ViewModel;

public class CurrentWorkingDirectoryTests
{
    public CurrentWorkingDirectoryTests()
    {
        if (!Directory.Exists(TestConstants.VALID_FOLDER_PATH))
        {
            Directory.CreateDirectory(TestConstants.VALID_FOLDER_PATH);
        }
    }

    /// <summary>
    /// CurrentWorkingDirectory HAS:
    /// CurrentDirectoryInfo
    /// CurrentDirPath
    /// CwdFiles
    /// FirstGeneration
    /// Status
    /// </summary>
    [Fact]
    public void Initialize_InvalidStartPath_HasNonEmptyStatus()
    {
        // Arrange
        var startingPath = TestConstants.INVALID_FOLDER_PATH;

        // Act
        var cwd = new CurrentWorkingDirectory(startingPath);
        string? expectedCurrentDirPath = string.Empty;
        var expectedStatus = "directory not found";

        // Assert
        Assert.Null(cwd.CurrentDirectoryInfo);
        Assert.Equal(expectedCurrentDirPath, cwd.CurrentDirPath);
        Assert.StartsWith(expectedStatus, cwd.Status);
    }

    [Fact]
    public void Initialize_ValidStartPath_EmptyStatus()
    {
        // Arrange
        var startingPath = TestConstants.VALID_FOLDER_PATH;

        // Act
        var cwd = new CurrentWorkingDirectory(startingPath);
        string? expectedCurrentDirPath = startingPath;
        var expectedFolderViewModelCount = 1;

        // Assert
        Assert.NotNull(cwd.CurrentDirectoryInfo);
        Assert.Equal(expectedCurrentDirPath, cwd.CurrentDirPath);
        Assert.Equal(expectedFolderViewModelCount, cwd.FirstGeneration.Count);
    }
}