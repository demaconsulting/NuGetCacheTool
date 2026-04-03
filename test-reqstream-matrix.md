# Summary

27 of 36 requirements are satisfied with tests.

# Requirements

## NuGet Cache Tool Requirements

### CLI

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-Cli-VersionFlag | 5 | 5 | 0 | 0 |
| NuGetCache-Cli-HelpFlag | 5 | 5 | 0 | 0 |
| NuGetCache-Cli-SilentFlag | 3 | 3 | 0 | 0 |
| NuGetCache-Cli-CachePackages | 3 | 2 | 0 | 1 |
| NuGetCache-Cli-ErrorOutput | 4 | 4 | 0 | 0 |
| NuGetCache-Cli-InvalidArguments | 4 | 4 | 0 | 0 |

### Context

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-Context-ArgumentParsing | 11 | 11 | 0 | 0 |
| NuGetCache-Context-SilentOutput | 5 | 5 | 0 | 0 |
| NuGetCache-Context-LogFile | 4 | 4 | 0 | 0 |
| NuGetCache-Context-ErrorTracking | 2 | 2 | 0 | 0 |
| NuGetCache-Context-InvalidArguments | 5 | 5 | 0 | 0 |

### SelfTest

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-SelfTest-Validation | 2 | 2 | 0 | 0 |
| NuGetCache-SelfTest-ResultsFile | 2 | 2 | 0 | 0 |
| NuGetCache-SelfTest-SafePathCombine | 7 | 7 | 0 | 0 |

### Validation

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-Validation-SelfValidation | 2 | 2 | 0 | 0 |
| NuGetCache-Validation-ResultsFile | 2 | 2 | 0 | 0 |

### PathHelpers

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-PathHelpers-SafePathCombine | 8 | 8 | 0 | 0 |

### Program

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-Program-VersionDisplay | 3 | 3 | 0 | 0 |
| NuGetCache-Program-HelpDisplay | 2 | 2 | 0 | 0 |
| NuGetCache-Program-CachePackages | 3 | 2 | 0 | 1 |
| NuGetCache-Program-Banner | 1 | 1 | 0 | 0 |
| NuGetCache-Program-ErrorOutput | 3 | 3 | 0 | 0 |

### Platform Support

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-PLT-Windows | 2 | 0 | 0 | 2 |
| NuGetCache-PLT-Linux | 2 | 0 | 0 | 2 |
| NuGetCache-PLT-MacOS | 2 | 0 | 0 | 2 |
| NuGetCache-PLT-Net8 | 2 | 0 | 0 | 2 |
| NuGetCache-PLT-Net9 | 2 | 0 | 0 | 2 |
| NuGetCache-PLT-Net10 | 2 | 0 | 0 | 2 |

### System Integration

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-Sys-Integration | 6 | 3 | 0 | 3 |

## OTS Software Requirements

### MSTest Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-OTS-MSTest | 9 | 9 | 0 | 0 |

### ReqStream Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-OTS-ReqStream | 1 | 1 | 0 | 0 |

### BuildMark Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-OTS-BuildMark | 1 | 1 | 0 | 0 |

### VersionMark Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-OTS-VersionMark | 2 | 2 | 0 | 0 |

### SarifMark Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-OTS-SarifMark | 2 | 2 | 0 | 0 |

### SonarMark Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-OTS-SonarMark | 4 | 4 | 0 | 0 |

### ReviewMark Requirements

| ID | Tests Linked | Passed | Failed | Not Executed |
| :- | -----------: | :-: | :-: | :-: |
| NuGetCache-OTS-ReviewMark | 2 | 2 | 0 | 0 |

# Testing

| Test | Requirement | Passed | Failed |
|------|-------------|--------|--------|
| BuildMark_MarkdownReportGeneration | NuGetCache-OTS-BuildMark | 1 | 0 |
| Context_Create_HelpFlag_SetsHelpTrue | NuGetCache-Cli-HelpFlag | 3 | 0 |
| Context_Create_HelpFlag_SetsHelpTrue | NuGetCache-Context-ArgumentParsing | 3 | 0 |
| Context_Create_HelpFlag_SetsHelpTrue | NuGetCache-OTS-MSTest | 3 | 0 |
| Context_Create_LogFlag_OpensLogFile | NuGetCache-Context-ArgumentParsing | 3 | 0 |
| Context_Create_LogFlag_OpensLogFile | NuGetCache-Context-LogFile | 3 | 0 |
| Context_Create_LogFlag_OpensLogFile | NuGetCache-OTS-MSTest | 3 | 0 |
| Context_Create_LogFlag_WithoutValue_ThrowsArgumentException | NuGetCache-Cli-InvalidArguments | 3 | 0 |
| Context_Create_LogFlag_WithoutValue_ThrowsArgumentException | NuGetCache-Context-InvalidArguments | 3 | 0 |
| Context_Create_NoArguments_ReturnsDefaultContext | NuGetCache-Context-ArgumentParsing | 3 | 0 |
| Context_Create_NoArguments_ReturnsDefaultContext | NuGetCache-OTS-MSTest | 3 | 0 |
| Context_Create_PackageArgument_AddsToPackagesList | NuGetCache-Cli-CachePackages | 3 | 0 |
| Context_Create_PackageArgument_AddsToPackagesList | NuGetCache-Context-ArgumentParsing | 3 | 0 |
| Context_Create_PackageArgument_AddsToPackagesList | NuGetCache-Program-CachePackages | 3 | 0 |
| Context_Create_ResultsFlag_SetsResultsFile | NuGetCache-Context-ArgumentParsing | 3 | 0 |
| Context_Create_ResultsFlag_SetsResultsFile | NuGetCache-OTS-MSTest | 3 | 0 |
| Context_Create_ResultsFlag_WithoutValue_ThrowsArgumentException | NuGetCache-Cli-InvalidArguments | 3 | 0 |
| Context_Create_ResultsFlag_WithoutValue_ThrowsArgumentException | NuGetCache-Context-InvalidArguments | 3 | 0 |
| Context_Create_ShortHelpFlag_H_SetsHelpTrue | NuGetCache-Cli-HelpFlag | 3 | 0 |
| Context_Create_ShortHelpFlag_H_SetsHelpTrue | NuGetCache-Context-ArgumentParsing | 3 | 0 |
| Context_Create_ShortHelpFlag_Question_SetsHelpTrue | NuGetCache-Cli-HelpFlag | 3 | 0 |
| Context_Create_ShortHelpFlag_Question_SetsHelpTrue | NuGetCache-Context-ArgumentParsing | 3 | 0 |
| Context_Create_ShortVersionFlag_SetsVersionTrue | NuGetCache-Cli-VersionFlag | 3 | 0 |
| Context_Create_ShortVersionFlag_SetsVersionTrue | NuGetCache-Context-ArgumentParsing | 3 | 0 |
| Context_Create_ShortVersionFlag_SetsVersionTrue | NuGetCache-OTS-MSTest | 3 | 0 |
| Context_Create_SilentFlag_SetsSilentTrue | NuGetCache-Cli-SilentFlag | 3 | 0 |
| Context_Create_SilentFlag_SetsSilentTrue | NuGetCache-Context-ArgumentParsing | 3 | 0 |
| Context_Create_SilentFlag_SetsSilentTrue | NuGetCache-Context-SilentOutput | 3 | 0 |
| Context_Create_SilentFlag_SetsSilentTrue | NuGetCache-OTS-MSTest | 3 | 0 |
| Context_Create_UnknownArgument_ThrowsArgumentException | NuGetCache-Cli-InvalidArguments | 3 | 0 |
| Context_Create_UnknownArgument_ThrowsArgumentException | NuGetCache-Context-InvalidArguments | 3 | 0 |
| Context_Create_UnknownArgument_ThrowsArgumentException | NuGetCache-OTS-MSTest | 3 | 0 |
| Context_Create_ValidateFlag_SetsValidateTrue | NuGetCache-Context-ArgumentParsing | 3 | 0 |
| Context_Create_ValidateFlag_SetsValidateTrue | NuGetCache-OTS-MSTest | 3 | 0 |
| Context_Create_VersionFlag_SetsVersionTrue | NuGetCache-Cli-VersionFlag | 3 | 0 |
| Context_Create_VersionFlag_SetsVersionTrue | NuGetCache-Context-ArgumentParsing | 3 | 0 |
| Context_Create_VersionFlag_SetsVersionTrue | NuGetCache-OTS-MSTest | 3 | 0 |
| Context_WriteError_NotSilent_WritesToConsole | NuGetCache-Cli-ErrorOutput | 3 | 0 |
| Context_WriteError_NotSilent_WritesToConsole | NuGetCache-Context-ErrorTracking | 3 | 0 |
| Context_WriteError_SetsErrorExitCode | NuGetCache-Cli-ErrorOutput | 3 | 0 |
| Context_WriteError_SetsErrorExitCode | NuGetCache-Context-ErrorTracking | 3 | 0 |
| Context_WriteError_Silent_DoesNotWriteToConsole | NuGetCache-Context-SilentOutput | 3 | 0 |
| Context_WriteError_WritesToLogFile | NuGetCache-Context-LogFile | 3 | 0 |
| Context_WriteLine_NotSilent_WritesToConsole | NuGetCache-Context-SilentOutput | 3 | 0 |
| Context_WriteLine_Silent_DoesNotWriteToConsole | NuGetCache-Cli-SilentFlag | 3 | 0 |
| Context_WriteLine_Silent_DoesNotWriteToConsole | NuGetCache-Context-SilentOutput | 3 | 0 |
| dotnet10.x@NuGetCache_HelpDisplay | NuGetCache-PLT-Net10 | 0 | 0 |
| dotnet10.x@NuGetCache_VersionDisplay | NuGetCache-PLT-Net10 | 0 | 0 |
| dotnet8.x@NuGetCache_HelpDisplay | NuGetCache-PLT-Net8 | 0 | 0 |
| dotnet8.x@NuGetCache_VersionDisplay | NuGetCache-PLT-Net8 | 0 | 0 |
| dotnet9.x@NuGetCache_HelpDisplay | NuGetCache-PLT-Net9 | 0 | 0 |
| dotnet9.x@NuGetCache_VersionDisplay | NuGetCache-PLT-Net9 | 0 | 0 |
| IntegrationTest_CacheNonexistentPackage_ReturnsError | NuGetCache-Cli-ErrorOutput | 3 | 0 |
| IntegrationTest_CacheNonexistentPackage_ReturnsError | NuGetCache-Program-ErrorOutput | 3 | 0 |
| IntegrationTest_CachePackage_OutputsPath | NuGetCache-Cli-CachePackages | 3 | 0 |
| IntegrationTest_CachePackage_OutputsPath | NuGetCache-Program-CachePackages | 3 | 0 |
| IntegrationTest_CachePackage_OutputsPath | NuGetCache-Sys-Integration | 3 | 0 |
| IntegrationTest_HelpFlag_OutputsUsageInformation | NuGetCache-Cli-HelpFlag | 3 | 0 |
| IntegrationTest_HelpFlag_OutputsUsageInformation | NuGetCache-Program-HelpDisplay | 3 | 0 |
| IntegrationTest_HelpFlag_OutputsUsageInformation | NuGetCache-Sys-Integration | 3 | 0 |
| IntegrationTest_LogFlag_WithInvalidFilename_ReturnsError | NuGetCache-Context-LogFile | 3 | 0 |
| IntegrationTest_LogFlag_WritesOutputToFile | NuGetCache-Context-LogFile | 3 | 0 |
| IntegrationTest_SilentFlag_SuppressesOutput | NuGetCache-Cli-SilentFlag | 3 | 0 |
| IntegrationTest_SilentFlag_SuppressesOutput | NuGetCache-Context-SilentOutput | 3 | 0 |
| IntegrationTest_UnknownArgument_ReturnsError | NuGetCache-Cli-ErrorOutput | 3 | 0 |
| IntegrationTest_UnknownArgument_ReturnsError | NuGetCache-Cli-InvalidArguments | 3 | 0 |
| IntegrationTest_UnknownArgument_ReturnsError | NuGetCache-Context-InvalidArguments | 3 | 0 |
| IntegrationTest_UnknownArgument_ReturnsError | NuGetCache-Program-ErrorOutput | 3 | 0 |
| IntegrationTest_ValidateFlag_RunsValidation | NuGetCache-SelfTest-Validation | 3 | 0 |
| IntegrationTest_ValidateFlag_RunsValidation | NuGetCache-Validation-SelfValidation | 3 | 0 |
| IntegrationTest_ValidateWithResults_GeneratesJUnitFile | NuGetCache-SelfTest-ResultsFile | 3 | 0 |
| IntegrationTest_ValidateWithResults_GeneratesJUnitFile | NuGetCache-Validation-ResultsFile | 3 | 0 |
| IntegrationTest_ValidateWithResults_GeneratesTrxFile | NuGetCache-SelfTest-ResultsFile | 3 | 0 |
| IntegrationTest_ValidateWithResults_GeneratesTrxFile | NuGetCache-Validation-ResultsFile | 3 | 0 |
| IntegrationTest_VersionFlag_OutputsVersion | NuGetCache-Cli-VersionFlag | 3 | 0 |
| IntegrationTest_VersionFlag_OutputsVersion | NuGetCache-Program-VersionDisplay | 3 | 0 |
| IntegrationTest_VersionFlag_OutputsVersion | NuGetCache-Sys-Integration | 3 | 0 |
| macos@NuGetCache_HelpDisplay | NuGetCache-PLT-MacOS | 0 | 0 |
| macos@NuGetCache_VersionDisplay | NuGetCache-PLT-MacOS | 0 | 0 |
| NuGetCache_CachePackage | NuGetCache-Cli-CachePackages | 0 | 0 |
| NuGetCache_CachePackage | NuGetCache-Program-CachePackages | 0 | 0 |
| NuGetCache_CachePackage | NuGetCache-Sys-Integration | 0 | 0 |
| NuGetCache_HelpDisplay | NuGetCache-Sys-Integration | 0 | 0 |
| NuGetCache_VersionDisplay | NuGetCache-Sys-Integration | 0 | 0 |
| PathHelpers_SafePathCombine_AbsolutePath_ThrowsArgumentException | NuGetCache-PathHelpers-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_AbsolutePath_ThrowsArgumentException | NuGetCache-SelfTest-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_CurrentDirectoryReference_CombinesCorrectly | NuGetCache-PathHelpers-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_CurrentDirectoryReference_CombinesCorrectly | NuGetCache-SelfTest-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_DoubleDotPrefix_CombinesCorrectly | NuGetCache-PathHelpers-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_DoubleDotsInMiddle_ThrowsArgumentException | NuGetCache-PathHelpers-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_DoubleDotsInMiddle_ThrowsArgumentException | NuGetCache-SelfTest-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_EmptyRelativePath_ReturnsBasePath | NuGetCache-PathHelpers-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_EmptyRelativePath_ReturnsBasePath | NuGetCache-SelfTest-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_NestedPaths_CombinesCorrectly | NuGetCache-PathHelpers-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_NestedPaths_CombinesCorrectly | NuGetCache-SelfTest-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_PathTraversalWithDoubleDots_ThrowsArgumentException | NuGetCache-PathHelpers-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_PathTraversalWithDoubleDots_ThrowsArgumentException | NuGetCache-SelfTest-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_ValidPaths_CombinesCorrectly | NuGetCache-PathHelpers-SafePathCombine | 3 | 0 |
| PathHelpers_SafePathCombine_ValidPaths_CombinesCorrectly | NuGetCache-SelfTest-SafePathCombine | 3 | 0 |
| Program_Run_NoArguments_DisplaysDefaultBehavior | NuGetCache-Program-Banner | 3 | 0 |
| Program_Run_WithHelpFlag_DisplaysUsageInformation | NuGetCache-Cli-HelpFlag | 3 | 0 |
| Program_Run_WithHelpFlag_DisplaysUsageInformation | NuGetCache-Program-HelpDisplay | 3 | 0 |
| Program_Run_WithInvalidPackageFormat_ThrowsArgumentException | NuGetCache-Context-InvalidArguments | 3 | 0 |
| Program_Run_WithValidateAndUnsupportedResultsFormat_SetsErrorExitCode | NuGetCache-Program-ErrorOutput | 3 | 0 |
| Program_Run_WithValidateFlag_RunsValidation | NuGetCache-SelfTest-Validation | 3 | 0 |
| Program_Run_WithValidateFlag_RunsValidation | NuGetCache-Validation-SelfValidation | 3 | 0 |
| Program_Run_WithVersionFlag_DisplaysVersionOnly | NuGetCache-Cli-VersionFlag | 3 | 0 |
| Program_Run_WithVersionFlag_DisplaysVersionOnly | NuGetCache-Program-VersionDisplay | 3 | 0 |
| Program_Version_ReturnsNonEmptyString | NuGetCache-Cli-VersionFlag | 3 | 0 |
| Program_Version_ReturnsNonEmptyString | NuGetCache-Program-VersionDisplay | 3 | 0 |
| ReqStream_EnforcementMode | NuGetCache-OTS-ReqStream | 1 | 0 |
| ReviewMark_ReviewPlanGeneration | NuGetCache-OTS-ReviewMark | 1 | 0 |
| ReviewMark_ReviewReportGeneration | NuGetCache-OTS-ReviewMark | 1 | 0 |
| SarifMark_MarkdownReportGeneration | NuGetCache-OTS-SarifMark | 1 | 0 |
| SarifMark_SarifReading | NuGetCache-OTS-SarifMark | 1 | 0 |
| SonarMark_HotSpotsRetrieval | NuGetCache-OTS-SonarMark | 1 | 0 |
| SonarMark_IssuesRetrieval | NuGetCache-OTS-SonarMark | 1 | 0 |
| SonarMark_MarkdownReportGeneration | NuGetCache-OTS-SonarMark | 1 | 0 |
| SonarMark_QualityGateRetrieval | NuGetCache-OTS-SonarMark | 1 | 0 |
| ubuntu@NuGetCache_HelpDisplay | NuGetCache-PLT-Linux | 0 | 0 |
| ubuntu@NuGetCache_VersionDisplay | NuGetCache-PLT-Linux | 0 | 0 |
| VersionMark_CapturesVersions | NuGetCache-OTS-VersionMark | 1 | 0 |
| VersionMark_GeneratesMarkdownReport | NuGetCache-OTS-VersionMark | 1 | 0 |
| windows@NuGetCache_HelpDisplay | NuGetCache-PLT-Windows | 0 | 0 |
| windows@NuGetCache_VersionDisplay | NuGetCache-PLT-Windows | 0 | 0 |

