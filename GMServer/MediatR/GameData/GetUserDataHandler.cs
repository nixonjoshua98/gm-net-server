﻿using GMServer.Models.UserModels;
using GMServer.Services;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GMServer.MediatR
{
    public class GetUserDataRequest : IRequest<GetUserDataResponse>
    {
        public string UserID { get; set; }
    }

    public class GetUserDataResponse
    {
        public List<UserArtefact> Artefacts { get; set; }
        public List<UserArmouryItem> ArmouryItems { get; set; }
        public UserCurrencies Currencies { get; set; }
        public List<UserMerc> UnlockedMercs { get; set; }
        public UserBounties Bounties { get; set; }
    }

    public class GetUserDataHandler : IRequestHandler<GetUserDataRequest, GetUserDataResponse>
    {
        private readonly ArtefactsService _artefacts;
        private readonly ArmouryService _armoury;
        private readonly CurrenciesService _currencies;
        private readonly MercService _mercs;
        private readonly BountiesService _bounties;

        public GetUserDataHandler(ArtefactsService artefacts, ArmouryService armoury, CurrenciesService currencies, MercService mercs, BountiesService bounties)
        {
            _artefacts = artefacts;
            _armoury = armoury;
            _currencies = currencies;
            _mercs = mercs;
            _bounties = bounties;
        }

        public async Task<GetUserDataResponse> Handle(GetUserDataRequest request, CancellationToken cancellationToken)
        {
            GetUserDataResponse response = new()
            {
                Artefacts = await _artefacts.GetUserArtefactsAsync(request.UserID),
                ArmouryItems = await _armoury.GetUserArmouryItemsAsync(request.UserID),
                Currencies = await _currencies.GetUserCurrenciesAsync(request.UserID),
                UnlockedMercs = await _mercs.GetUserMercsAsync(request.UserID),
                Bounties = await _bounties.GetUserBountiesAsync(request.UserID),
            };

            return response;
        }
    }
}