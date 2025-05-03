import { createContext, useContext } from "react";
import { SystemSettings } from "../types/settingsTypes";

export const SystemSettingsContext = createContext<SystemSettings | null>(null);

export const useSystemSettings = () => {
  const context = useContext(SystemSettingsContext);
  if (!context) throw new Error("SystemSettingsContext fehlt");
  return context;
};
