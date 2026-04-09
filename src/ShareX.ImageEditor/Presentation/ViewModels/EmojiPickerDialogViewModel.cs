#region License Information (GPL v3)

/*
    ShareX.ImageEditor - The UI-agnostic Editor library for ShareX
    Copyright (c) 2007-2026 ShareX Team

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShareX.ImageEditor.Presentation.Emoji;
using System.Collections.ObjectModel;

namespace ShareX.ImageEditor.Presentation.ViewModels;

public partial class EmojiPickerDialogViewModel : ObservableObject
{
    private readonly IReadOnlyList<EmojiCatalogEntry> _catalog;
    private readonly Action<EmojiCatalogEntry> _onSelect;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedGroup = string.Empty;

    [ObservableProperty]
    private ObservableCollection<EmojiCatalogEntry> _visibleEmojis = [];

    [ObservableProperty]
    private string _resultsSummary = string.Empty;

    [ObservableProperty]
    private string _searchWatermark = "Search emojis";

    public IReadOnlyList<string> GroupOptions { get; }

    public bool HasResults => VisibleEmojis.Count > 0;

    public IRelayCommand<EmojiCatalogEntry?> SelectEmojiCommand { get; }

    public IRelayCommand CancelCommand { get; }

    public EmojiPickerDialogViewModel(Action<EmojiCatalogEntry> onSelect, Action onCancel)
    {
        _catalog = EmojiCatalogService.GetCatalog();
        _onSelect = onSelect;

        GroupOptions = EmojiCatalogService.GetGroups();
        SelectedGroup = GroupOptions.FirstOrDefault() ?? string.Empty;

        SelectEmojiCommand = new RelayCommand<EmojiCatalogEntry?>(SelectEmoji);
        CancelCommand = new RelayCommand(onCancel);

        RefreshResults();
    }

    partial void OnSearchTextChanged(string value) => RefreshResults();

    partial void OnSelectedGroupChanged(string value) => RefreshResults();

    private void SelectEmoji(EmojiCatalogEntry? emoji)
    {
        if (emoji == null)
        {
            return;
        }

        _onSelect(emoji);
    }

    private void RefreshResults()
    {
        string search = SearchText.Trim();
        EmojiCatalogEntry[] categoryEntries =
        [
            .. _catalog.Where(entry => string.Equals(entry.Group, SelectedGroup, StringComparison.Ordinal))
        ];

        SearchWatermark = $"Search emojis... ({categoryEntries.Length})";

        IEnumerable<EmojiCatalogEntry> query;

        if (string.IsNullOrWhiteSpace(search))
        {
            query = categoryEntries;

            ResultsSummary = string.IsNullOrEmpty(SelectedGroup)
                ? "Browse emojis"
                : $"{SelectedGroup} - {query.Count()} emojis";
        }
        else
        {
            query = categoryEntries
                .Select(entry => (Entry: entry, Score: entry.GetSearchScore(search)))
                .Where(match => match.Score != int.MaxValue)
                .OrderBy(match => match.Score)
                .Select(match => match.Entry);

            ResultsSummary = $"Search results - {query.Count()} matches";
        }

        VisibleEmojis = [.. query];
        OnPropertyChanged(nameof(HasResults));
    }
}
