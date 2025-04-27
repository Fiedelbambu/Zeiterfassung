// src/api/UserApiClient.ts
export interface UserDTO {
    id: string;
    username: string;
    email: string;
    role: string;
  }
  
  const API_BASE_URL = "https://localhost:7123/api"; // Dein Backend-URL
  
  export async function fetchUsers(): Promise<UserDTO[]> {
    const response = await fetch(`${API_BASE_URL}/UserAdmin`, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${localStorage.getItem('jwt_token')}`,
      },
    });
  
    if (!response.ok) {
      throw new Error("Benutzer konnten nicht geladen werden");
    }
  
    return await response.json();
  }
  