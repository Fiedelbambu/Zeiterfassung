import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
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
  return (
    <Router>
      <Routes>
        {/* Login Route */}
        <Route path="/" element={<LoginForm />} />

        {/* Admin Dashboard mit Unterrouten */}
        <Route path="/dashboard" element={<Dashboard />}>
          <Route path="users" element={<UsersPage />} />
          <Route path="times" element={<TimesPage />} />
          <Route path="absences" element={<AbsencesPage />} />
          <Route path="reports" element={<ReportsPage />} />
          <Route path="settings" element={<SettingsPage />} />
          <Route path="admin-settings" element={<AdminSettingsPage />} />{" "}
          
        </Route>

        {/* Mitarbeiter-Dashboard */}
        <Route path="/employee-dashboard" element={<EmployeeDashboard />} />
      </Routes>
    </Router>
  );
}
