"use client";

import React, { useState } from 'react';
import axios from 'axios';

// types to match the backend expectation
interface SummaryResponse {
  id: string; // The Guid returned by the API
}

export default function DossierPage() {
  const [noteContent, setNoteContent] = useState('');
  const [summary, setSummary] = useState<string | null>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  // Mock Dossier ID for testing
  const dossierId = "3fa85f64-5717-4562-b3fc-2c963f66afa6"; 
  const API_BASE_URL = "http://localhost:53332/api"; // Updated to match HTTP port in launchSettings.json

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!noteContent.trim()) return;

    setIsLoading(true);
    setError(null);
    setSummary(null);

    try {
      // Ensure we are hitting the HTTP endpoint exactly
      const response = await axios.post("http://localhost:53332/api/dossier/3fa85f64-5717-4562-b3fc-2c963f66afa6/notes", 
        JSON.stringify(noteContent), 
        {
          headers: {
            'Content-Type': 'application/json'
          }
        }
      );

      // In a real app, we might fetch the specific note details again 
      // but here we just confirm success or display a message
      setSummary("Note successfully processed and saved to the EPR. (AI Summarization handled server-side)");
    } catch (err: any) {
      console.error(err);
      setError(err.response?.data?.title || "Failed to connect to the Dossier API. Ensure the backend is running and CORS is enabled.");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <main className="min-h-screen bg-slate-50 p-4 md:p-8 font-sans">
      <div className="max-w-3xl mx-auto bg-white rounded-xl shadow-sm border border-slate-200 overflow-hidden">
        {/* Header */}
        <header className="bg-blue-600 p-6 text-white">
          <h1 className="text-2xl font-bold">Patient Dossier: John Doe</h1>
          <p className="text-blue-100 text-sm mt-1">Patient ID: 12345-ABC | DOB: 1980-05-15</p>
        </header>

        <div className="p-6 space-y-6">
          {/* Form */}
          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label htmlFor="note" className="block text-sm font-semibold text-slate-700 mb-2">
                Session Note (Raw Data)
              </label>
              <textarea
                id="note"
                rows={8}
                className="w-full p-4 border border-slate-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent outline-none transition-all text-slate-800 placeholder:text-slate-400"
                placeholder="Type the messy, unstructured session note here..."
                value={noteContent}
                onChange={(e) => setNoteContent(e.target.value)}
                disabled={isLoading}
              />
            </div>

            <button
              type="submit"
              disabled={isLoading || !noteContent.trim()}
              className={`w-full py-3 px-4 rounded-lg font-bold text-white transition-all ${
                isLoading 
                  ? 'bg-slate-400 cursor-not-allowed' 
                  : 'bg-blue-600 hover:bg-blue-700 active:scale-[0.98]'
              }`}
            >
              {isLoading ? (
                <span className="flex items-center justify-center gap-2">
                  <svg className="animate-spin h-5 w-5 text-white" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                  </svg>
                  Processing...
                </span>
              ) : "Submit Note & Generate Summary"}
            </button>
          </form>

          {/* Error State */}
          {error && (
            <div className="p-4 bg-red-50 border border-red-200 text-red-700 rounded-lg text-sm">
              <strong>Error:</strong> {error}
            </div>
          )}

          {/* Results Section */}
          {summary && (
            <div className="mt-8 space-y-4 animate-in fade-in slide-in-from-bottom-4 duration-500">
              <h2 className="text-lg font-bold text-slate-900 flex items-center gap-2">
                <span className="bg-green-100 text-green-700 p-1 rounded">✓</span>
                Processing Complete
              </h2>
              <div className="bg-slate-50 border border-slate-200 rounded-lg p-6">
                <h3 className="text-sm font-bold text-slate-500 uppercase tracking-wider mb-2">AI Generated Insight</h3>
                <p className="text-slate-700 leading-relaxed italic">
                  "{summary}"
                </p>
                <div className="mt-4 pt-4 border-t border-slate-200 text-xs text-slate-400">
                  Note stored successfully in Patient Dossier database.
                </div>
              </div>
            </div>
          )}
        </div>
      </div>

      <footer className="mt-8 text-center text-slate-400 text-xs">
        &copy; 2026 ModernEPR Systems | Dossier Modernization Module
      </footer>
    </main>
  );
}
