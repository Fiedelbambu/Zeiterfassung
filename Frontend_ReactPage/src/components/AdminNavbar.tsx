// src/components/AdminNavbar.tsx
import { NavLink, useNavigate } from 'react-router-dom';
import { clearSession } from '../utils/auth';

export default function AdminNavbar() {
  const navigate = useNavigate();

  const handleLogout = () => {
    const confirmed = window.confirm("Möchtest du dich wirklich abmelden?");
    if (confirmed) {
      clearSession();
      navigate('/');
    }
  };
  
  return (
    <aside className="w-64 bg-blue-900 text-white flex flex-col p-6 min-h-screen">
      <h2 className="text-2xl font-bold mb-10 text-center">Admin Panel</h2>

      <nav className="flex flex-col flex-grow space-y-2">
        <NavLink
          to="/dashboard/users"
          className={({ isActive }) =>
            `p-2 rounded transition ${isActive ? 'bg-blue-700' : 'hover:bg-blue-700'}`
          }
        >
          Benutzerverwaltung
        </NavLink>

        <NavLink
          to="/dashboard/times"
          className={({ isActive }) =>
            `p-2 rounded transition ${isActive ? 'bg-blue-700' : 'hover:bg-blue-700'}`
          }
        >
          Zeitbuchungen
        </NavLink>

        <NavLink
          to="/dashboard/absences"
          className={({ isActive }) =>
            `p-2 rounded transition ${isActive ? 'bg-blue-700' : 'hover:bg-blue-700'}`
          }
        >
          Abwesenheiten
        </NavLink>

        <NavLink
          to="/dashboard/reports"
          className={({ isActive }) =>
            `p-2 rounded transition ${isActive ? 'bg-blue-700' : 'hover:bg-blue-700'}`
          }
        >
          Berichte
        </NavLink>

        <NavLink
          to="/dashboard/settings"
          className={({ isActive }) =>
            `p-2 rounded transition ${isActive ? 'bg-blue-700' : 'hover:bg-blue-700'}`
          }
        >
          Einstellungen
        </NavLink>
      </nav>

     {/* Logout unten fixiert */}
  <div className="mt-8">
    <button
      onClick={handleLogout}
      className="w-full bg-red-500 hover:bg-red-600 text-white p-2 rounded"
    >
      Logout
    </button>
  </div>
</aside>
  );
}
