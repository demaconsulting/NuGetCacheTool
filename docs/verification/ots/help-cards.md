## DemaConsulting.HelpCards Verification

This document provides the verification evidence for the `DemaConsulting.HelpCards` OTS software
item. Requirements for this OTS item are defined in the HelpCards OTS Software Requirements
document.

### Required Functionality

`DemaConsulting.HelpCards` (`NuGetCache-OTS-HelpCards-Cli` and `NuGetCache-OTS-HelpCards-Testing`)
shall answer the help command of the tool from the cards embedded in the tool assembly, and report
card defects and example results to the tests of the tool. See
*DemaConsulting.HelpCards Integration Design* for the integration pattern.

### Verification Approach

`DemaConsulting.HelpCards` is a runtime library with no self-validation mode, so it is verified by
test evidence from the tests of the tool, which use the real package and the cards embedded in the
tool assembly.

### Test Scenarios

#### HelpCommand_TryRun_HelpVerb_PrintsTopicIndexAndReturnsZero

**Scenario**: The help verb is answered in-process from the embedded cards.

**Expected**: The topic index lists every root topic, with exit code 0.

#### NuGetCacheTool_HelpTopics_HelpVerb_ListsTopics

**Scenario**: The built tool is run with the help verb.

**Expected**: The topic index lists every root topic, with exit code 0.

#### HelpCommand_CreateOptions_ShippedCatalog_PassesValidation

**Scenario**: The validator checks the shipped cards with warnings treated as errors.

**Expected**: No defect is reported.

#### NuGetCacheTool_HelpExamples_AllCommandExamples_GiveStatedResults

**Scenario**: The example runner runs every command example against the built tool.

**Expected**: Every example gives the exit code and output that its card states.

### Acceptance Criteria

N/A - Acceptance criteria are managed at the system integration level. This OTS item is
considered verified when the test scenarios that exercise its functionality pass in the CI
pipeline.
