import axios from 'axios';
import { SystemSettings as SystemSettingsDTO } from '../types/settingsTypes';

// GET: Einstellungen abrufen
export const getSystemSettings = async (): Promise<SystemSettingsDTO> => {
  const response = await axios.get<SystemSettingsDTO>('/api/settings');
  return response.data;
};

// PUT: Einstellungen aktualisieren
export const updateSystemSettings = async (settings: SystemSettingsDTO): Promise<void> => {
  await axios.put('/api/settings', settings);
};
