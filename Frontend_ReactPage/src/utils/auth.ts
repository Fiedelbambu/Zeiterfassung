// src/utils/auth.ts
import { jwtDecode } from 'jwt-decode';

const TOKEN_KEY = 'jwt_token';
const ROLE_KEY = 'user_role';

interface TokenPayload {
  username: string;
  role?: string;
  [key: string]: any; // Beliebige andere Felder
}

/**
 * Holt den gespeicherten Token aus localStorage.
 */
export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY);
}

/**
 * Holt den gespeicherten Role-Eintrag aus localStorage.
 */
export function getRole(): string | null {
  return localStorage.getItem(ROLE_KEY);
}

/**
 * Speichert Token und Rolle in localStorage.
 */
export function saveSession(token: string, role: string) {
  localStorage.setItem(TOKEN_KEY, token);
  localStorage.setItem(ROLE_KEY, role);
}

/**
 * Löscht Token und Rolle beim Logout.
 */
export function clearSession() {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(ROLE_KEY);
}

/**
 * Holt den Benutzernamen aus dem gespeicherten Token.
 */
export function getUsernameFromToken(): string | null {
  const token = getToken();
  if (!token) return null;

  try {
    const payload = jwtDecode<TokenPayload>(token);
    return payload.username || null;
  } catch (err) {
    console.error("Token konnte nicht dekodiert werden:", err);
    return null;
  }
}
/**
 * Holt die Ablaufzeit des Tokens.
 */
export function getTokenExpiry(): number | null {
  const token = getToken();
  if (!token) return null;

  try {
    const payload = jwtDecode<{ exp: number }>(token);
    return payload.exp * 1000; // Sekunden → Millisekunden
  } catch (err) {
    console.error("Token Ablaufzeit konnte nicht gelesen werden:", err);
    return null;
  }
}


