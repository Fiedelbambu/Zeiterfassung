import { saveSession } from '../utils/auth';
import { jwtDecode } from 'jwt-decode'; // importieren
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import LoginImage from '../assets/BigPicture/ZeiterfassungLogin.webp';



export default function LoginForm() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

      try {
      const response = await fetch('https://localhost:7123/api/Auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, passwort: password })
      });

      if (!response.ok) {
        const result = await response.json();
        throw new Error(result.message || 'Unbekannter Fehler');
        }

      const data = await response.json();
      saveSession(data.token, ""); // Rolle wird gleich geholt, darum leer lassen

      // Token auslesen
      const decoded = jwtDecode<any>(data.token);
const role = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

if (role === "Admin") {
  navigate("/dashboard");
} else {
  navigate("/employee-dashboard");
}

    } catch (err: any) {
      console.error('Fehler beim Login:', err);
      setError('Login fehlgeschlagen. Bitte überprüfe deine Eingaben.');
    }
  };

  return (
    <div className="flex min-h-screen bg-gray-100">
      {/* Linke Seite: Formular */}
      <div className="flex-1 flex items-center justify-center p-8">
        <form onSubmit={handleLogin} className="bg-white p-8 rounded-lg shadow-md w-full max-w-sm">
          <h2 className="text-2xl font-bold mb-6 text-center">Login</h2>

          <div className="mb-4">
            <label htmlFor="username" className="block text-sm font-medium text-gray-700 mb-1">
              Benutzername
            </label>
            <input
              id="username"
              type="text"
              value={username}
              onChange={e => setUsername(e.target.value)}
              required
              className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring focus:border-blue-400"
            />
          </div>

          <div className="mb-6">
            <label htmlFor="password" className="block text-sm font-medium text-gray-700 mb-1">
              Passwort
            </label>
            <input
              id="password"
              type="password"
              value={password}
              onChange={e => setPassword(e.target.value)}
              required
              className="w-full border border-gray-300 rounded px-3 py-2 focus:outline-none focus:ring focus:border-blue-400"
            />
          </div>

          {error && <p className="text-red-600 mb-4">{error}</p>}

          <button
            type="submit"
            className="w-full bg-blue-600 text-white py-2 rounded hover:bg-blue-700 transition"
          >
            Einloggen
          </button>
        </form>
      </div>

      {/* Rechte Seite: Bild */}
      <div className="hidden md:flex flex-1 items-center justify-center p-8 animate-slide-in-right-blur">
        <img
          src={LoginImage}
          alt="Login Illustration"
          className="w-full h-auto object-cover rounded-lg shadow-lg"
        />
      </div>
    </div>
  );
}
