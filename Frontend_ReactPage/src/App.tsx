import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { useEffect } from "react";
import { getToken } from "./utils/auth";
import { useInitLanguage } from "./hooks/useInitLanguage"; 
import i18n from "./i18n"; 

import LoginForm from "./pages/LoginForm";
import Dashboard from "./pages/Dashboard";
import EmployeeDashboard from "./pages/EmployeeDashboard";
import AdminSettingsPage from "./pages/AdminSettingsPage";
import UsersPage from "./pages/UsersPage";
import TimesPage from "./pages/TimesPage";
import AbsencesPage from "./pages/AbsencesPage";
import ReportsPage from "./pages/ReportsPage";
import SettingsPage from "./pages/SettingsPage";

export default function App() {
  useInitLanguage(); // <--- Sprache beim Start setzen
  useEffect(() => {
    const token = getToken();
    if (!token) return;
  
    const loadLanguage = async () => {
      try {
        const res = await fetch("https://localhost:7123/api/settings", {
          headers: { Authorization: `Bearer ${token}` },
        });
        if (res.ok) {
          const data = await res.json();
          if (data.language) {
            i18n.changeLanguage(data.language);
            console.log("Sprache geladen:", data.language);
          }
        }
      } catch (err) {
        console.warn("Sprache konnte nicht geladen werden:", err);
      }
    };
  
    loadLanguage();
  }, []);
  
  return (
    <Router>
      <Routes>
        <Route path="/" element={<LoginForm />} />
        <Route path="/dashboard" element={<Dashboard />}>
          <Route path="users" element={<UsersPage />} />
          <Route path="times" element={<TimesPage />} />
          <Route path="absences" element={<AbsencesPage />} />
          <Route path="reports" element={<ReportsPage />} />
          <Route path="settings" element={<SettingsPage />} />
          <Route path="admin-settings" element={<AdminSettingsPage />} />
        </Route>
        <Route path="/employee-dashboard" element={<EmployeeDashboard />} />
      </Routes>
    </Router>
  );
}
