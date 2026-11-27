# 2. Expo managed workflow with development builds

- Status: Accepted
- Date: 2026-11-27

## Context

Step data comes from Android Health Connect through `react-native-health-connect`, which has native code and cannot run in Expo Go.

## Decision

Use the Expo managed workflow with Continuous Native Generation. Native configuration lives in `app.json` and config plugins; `android/` and `ios/` are generated and never committed. Development uses a dev client build; releases use the EAS `preview` profile to produce an APK.

## Consequences

- No native folders to maintain, and SDK upgrades stay simple.
- Any library with native code needs a new dev build.
- If a library ever needs manual native edits, we write a small config plugin instead of ejecting.
