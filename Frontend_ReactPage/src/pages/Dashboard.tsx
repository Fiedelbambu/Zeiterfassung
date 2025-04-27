import { Outlet, useNavigate } from 'react-router-dom';
import AdminNavbar from '../components/AdminNavbar';
import { getUsernameFromToken, getTokenExpiry, clearSession } from '../utils/auth';
import { useState, useEffect } from 'react';

export default function Dashboard() {
  const navigate = useNavigate();
  const username = getUsernameFromToken() ?? "Benutzer";

  const [remainingMinutes, setRemainingMinutes] = useState<number | null>(null);
  const [showModal, setShowModal] = useState(false);

  useEffect(() => {
    const interval = setInterval(() => {
      const expiry = getTokenExpiry();
      if (!expiry) {
        clearSession();
        navigate('/');
        return;
      }

      const diffMs = expiry - Date.now();
      const diffMin = Math.floor(diffMs / 60000);
      setRemainingMinutes(diffMin);

      if (diffMin <= 5 && diffMin >= 0) {
        setShowModal(true);
      }

      if (diffMs <= 0) {
        clearSession();
        navigate('/');
      }
    }, 30000); // alle 30 Sekunden prüfen

    return () => clearInterval(interval);
  }, [navigate]);

  return (
    <div className="flex min-h-screen">
      {/* Sidebar */}
      <AdminNavbar />

      {/* Hauptbereich */}
      <main className="flex-1 bg-gray-100 p-10 overflow-y-auto">
        {/* Begrüßung + Ablaufzeit */}
        <div className="mb-8 flex items-center justify-between">
          <h1 className="text-2xl font-bold">
            Willkommen, <span className="text-blue-700">{username}</span>!
          </h1>
          {remainingMinutes !== null && (
            <p className="text-gray-600 text-sm">
              Token läuft ab in <span className="font-semibold">{remainingMinutes}</span> Minuten
            </p>
          )}
        </div>

        {/* Hier werden die Unterseiten eingefügt */}
        <Outlet />

        {/* Modal */}
        {showModal && (
          <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
            <div className="bg-white p-8 rounded-lg shadow-md text-center max-w-md">
              <h2 className="text-xl font-bold mb-4">Sitzung läuft bald ab</h2>
              <p className="mb-6">Deine Sitzung läuft bald ab. Bitte speichere deine Arbeit!</p>
              <button
                onClick={() => setShowModal(false)}
                className="bg-blue-600 hover:bg-blue-700 text-white py-2 px-6 rounded"
              >
                OK
              </button>
            </div>
          </div>
        )}
      </main>
    </div>
  );
}
